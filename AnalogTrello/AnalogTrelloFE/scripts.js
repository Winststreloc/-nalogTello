const apiBaseUrl = "https://localhost:7127";
let isAuthenticated = false;

document.addEventListener("DOMContentLoaded", () => {
  updateUI();

  document
    .getElementById("register-form")
    .addEventListener("submit", async function (e) {
      e.preventDefault();
      const userName = document.getElementById("register-userName").value;
      const email = document.getElementById("register-email").value;
      const password = document.getElementById("register-password").value;
      const repeatPassword = document.getElementById(
        "register-repeat-password"
      ).value;

      if (password !== repeatPassword) {
        document.getElementById("register-error").classList.remove("d-none");
      } else {
        try {
          const response = await fetch(`${apiBaseUrl}/Auth/register`, {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({ userName, email, password }),
          });

          if (response.status === 204) {
            isAuthenticated = true;
            document.getElementById("user-email").innerText = email;
            updateUI();
            bootstrap.Modal.getInstance(
              document.getElementById("registerModal")
            ).hide();
          } else {
            document
              .getElementById("register-error")
              .classList.remove("d-none");
          }
        } catch (error) {
          document.getElementById("register-error").classList.remove("d-none");
        }
      }
    });

  document
    .getElementById("login-form")
    .addEventListener("submit", async function (e) {
      e.preventDefault();
      const userName = document.getElementById("login-userName").value;
      const password = document.getElementById("login-password").value;
      let email = " ";

      try {
        const response = await fetch(`${apiBaseUrl}/Auth/login`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ userName, email, password }),
        });

        if (response.ok === true) {
          const responseData = await response.json();

          if (responseData.isSuccess) {
            const { accessToken, refreshToken, username, userId } =
              responseData.result;

            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", refreshToken);
            localStorage.setItem("username", username);
            localStorage.setItem("userId", userId);

            isAuthenticated = true;
            document.getElementById("user-email").innerText = username;
            updateUI();
            bootstrap.Modal.getInstance(
              document.getElementById("loginModal")
            ).hide();

            await loadTasks();
          } else {
            document.getElementById("login-error").classList.remove("d-none");
          }
        } else {
          document.getElementById("login-error").classList.remove("d-none");
        }
      } catch (error) {
        console.log(error);
        document.getElementById("login-error").classList.remove("d-none");
      }
    });

  document
    .getElementById("logoutBtn")
    .addEventListener("click", async function () {
      await fetch(`${apiBaseUrl}/Auth/logout`, {
        // Обновили URL
        method: "POST",
      });
      isAuthenticated = false;
      updateUI();
    });

  document
    .getElementById("add-task-btn")
    .addEventListener("click", async function () {
      let taskTitle = document.getElementById("task-title-input").value;

      if (taskTitle.trim() === "") {
        alert("Task title cannot be empty");
        return;
      }

      try {
        const now = new Date(); // Текущее время
        const oneWeekLater = new Date(now.getTime() + 7 * 24 * 60 * 60 * 1000);

        const response = await fetch(`${apiBaseUrl}/TaskScheduler/task`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          },
          body: JSON.stringify({
            id: 0,
            title: taskTitle,
            text: "test",
            userId: localStorage.getItem("userId"),
            taskStatus: 2,
            endTimeTask: oneWeekLater.toISOString(),
          }),
        });

        if (response.ok) {
          const task = await response.json();
          addTaskToPendingList(task);
          document.getElementById("task-title-input").value = ""; // Clear input field
        } else {
          alert("Error adding task");
        }
      } catch (error) {
        alert("Error adding task");
      }

      function addTaskToPendingList(task) {
        const taskItem = document.createElement("li");
        taskItem.className = "list-group-item";

        taskItem.innerHTML = `
        <div class="d-flex justify-content-between align-items-center">
            <div>
                <strong>${task.title}</strong> <br>
                <small>${task.text}</small>
            </div>
            <div class="text-end">
                <small>End: ${new Date(
                  task.endTimeTask
                ).toLocaleString()}</small><br>
                <input type="checkbox" onchange="toggleTaskStatus(${
                  task.id
                }, true)" />
            </div>
        </div>
    `;

        document.getElementById("pending-tasks-list").appendChild(taskItem);
      }

      // Функция для добавления задачи в список выполненных задач
      function addTaskToCompletedList(task) {
        const taskItem = document.createElement("li");
        taskItem.className = "list-group-item";

        taskItem.innerHTML = `
        <div class="d-flex justify-content-between align-items-center">
            <div>
                <strong>${task.title}</strong> <br>
                <small>${task.text}</small>
            </div>
            <div class="text-end">
                <small>End: ${new Date(
                  task.endTimeTask
                ).toLocaleString()}</small><br>
                <input type="checkbox" checked onchange="toggleTaskStatus(${task}, false)" />
            </div>
        </div>
    `;

        document.getElementById("completed-tasks-list").appe
        ndChild(taskItem);
      }

      // Функция для изменения статуса задачи
      async function toggleTaskStatus(task, isCompleted) {
        try {
          task.status = isCompleted ? 2 : 1;

          const response = await fetch(`${apiBaseUrl}/TaskScheduler/task`, {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
            },
            body: JSON.stringify({
              task,
            }),
          });

          if (response.ok) {
            // Если статус обновился, обновляем списки задач
            await loadTasks();
          } else {
            alert("Failed to update task status");
          }
        } catch (error) {
          alert("Error updating task status");
        }
      }

      // Функция для удаления задачи
      async function deleteTask(taskId) {
        try {
          const response = await fetch(`${apiBaseUrl}/tasks/${taskId}`, {
            method: "DELETE",
            headers: {
              Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
            },
          });

          if (response.ok) {
            document.getElementById(`task-${taskId}`).parentElement.remove();
          } else {
            alert("Error deleting task");
          }
        } catch (error) {
          alert("Error deleting task");
        }
      }
    });

  // Функция для добавления задачи в список невыполненных задач

  // Функция для получения списка задач при загрузке страницы
  async function loadTasks() {
    try {
      const response = await fetch(`${apiBaseUrl}/TaskScheduler/tasks`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
        },
      });

      if (response.ok) {
        const data = await response.json();

        if (data.isSuccess) {
          const tasks = data.result;

          tasks.forEach((task) => {
            if (task.taskStatus === 1) {
              addTaskToCompletedList(task);
            } else {
              addTaskToPendingList(task);
            }
          });
        } else {
          alert(`Error: ${data.errorMessages}`);
        }
      } else {
        alert("Error loading tasks");
      }
    } catch (error) {
      alert("Error loading tasks");
    }
  }

  function updateUI() {
    if (isAuthenticated) {
      document.getElementById("auth-buttons").classList.add("d-none");
      document.getElementById("user-info").classList.remove("d-none");
      document.getElementById("content").classList.remove("d-none");
    } else {
      document.getElementById("auth-buttons").classList.remove("d-none");
      document.getElementById("user-info").classList.add("d-none");
      document.getElementById("content").classList.add("d-none");
    }
  }
});
