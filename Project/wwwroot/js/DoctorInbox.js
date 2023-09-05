const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Hubs/ChatHub").build();
var DoctorId;
var PatientId;
var DoctorName = "";
var PatientName = "";
var User_role = "Doctor";
function sendMessage(DoctorId, message, PatientId ) {
    var ms = message;
    console.log(typeof (ms));
    connection.invoke("SendMessage", parseInt(PatientId), (message), parseInt(DoctorId), (User_role)).catch(err => console.error("Error calling LoadChatFunctionDoctor:", err));
}
document.addEventListener("DOMContentLoaded", () => {
    const loadchatlink = document.querySelectorAll(".load-chat-link");
    loadchatlink.forEach(a => {
        a.addEventListener("click", function () {
            PatientId = a.getAttribute('data-patient-id');
            PatientName = a.getAttribute('data-patient-name');
            document.getElementById("ptn_name").textContent = PatientName;
            console.log("Here");
            console.log(PatientId);
            const DoctorInfo = getUserCookie('UserInfo');
        if (DoctorInfo) {
            const parsedObject = JSON.parse(DoctorInfo);
            const role = parsedObject.UserRole;
            const user = parsedObject.User;
            DoctorId = user.Id;
            DoctorName = user.Name;
            console.log(DoctorName);
            if (connection.state === signalR.HubConnectionState.Connected) {
                stopConnection().then(() => {
                    startConnection();
                });
            } else {
                startConnection();
            }
            function startConnection() {
                return connection.start().then(() => {
                    console.log("Connection started");
                    connection.invoke("LoadChatForDoctor", parseInt(DoctorId), parseInt(PatientId)).catch(err => console.error("Error calling get all messages :", err));
                    }).catch(err => console.error(err)); 
            }
            function stopConnection() {
                return connection.stop()
                    .then(() => {
                        console.log("Connection stopped");
                        clearMessageContainer();
                    })
                    .catch(err => console.error(err));
            }
            function clearMessageContainer() {
                const messageContainer = document.querySelector(".message-container");
                messageContainer.innerHTML = ""; // Clear the content
            }
        }
        else {
            window.location.href = "/Login_Signup/Login";
        }
        });
    });
    /////////////////////////////////////////////////////////////////
    document.getElementById("btn_send").addEventListener("click", () => {
        const message = document.getElementById("messageInput").value;
        const newParagraph = document.createElement("p");
        newParagraph.className = "sent";
        newParagraph.textContent = message;
        document.querySelector(".message-container").appendChild(newParagraph);
        sendMessage(parseInt(PatientId), message, parseInt(DoctorId), User_role);
        document.getElementById("messageInput").value = "";
    });
    if (connection.state === signalR.HubConnectionState.Connected) {
        stopConnection1().then(() => {
            startConnection();
        });
    } else {
        startConnection1();
    }
    function stopConnection1() {
        return connection.stop()
            .then(() => {
                console.log("Connection stopped");
                clearMessageContainer();
            })
            .catch(err => console.error(err));
    }
    function startConnection1() {
        return connection.start().then(() => {
            console.log("Connection started");
        }).catch(err => console.error(err));
    }
    connection.on("ReceiveMessageDoctor", (senderId, message) => {
        if (parseInt(senderId) == parseInt(DoctorId) && message.userRole !== User_role) {
            displayMessage(message.messageContent);
        }
        else {
            console.log("User not found");
        }
        function displayMessage(message) {
            if (message === null) {
                console.log("No message to display from ")
            }
            const newParagraph = document.createElement("p");
            newParagraph.className = "received-gradient";
            newParagraph.textContent = message;
            document.querySelector(".message-container").appendChild(newParagraph);
        }
    });
    function getUserCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return decodeURIComponent(parts.pop().split(';').shift());
    }
    connection.on("LoadChat", (dct_id, messages) => {
        if (parseInt(dct_id) === DoctorId)
        {
            if (messages === null)
            {
                console.log("No message in db");
            }
            else
            {
                console.log(messages);
                for (const message of messages) {
                    if (message.userRole === "Doctor") {
                        displayMessagePersonal(message.messageContent);
                    }
                    else if (message.userRole === "Patient") {
                        displayMessage(message.messageContent);
                    }
                }
            }
        }
        else {
            console.log("User not found");
        }
        function displayMessage(message) {
            if (message === null) {
                console.log("No message to display from ")
            }
            const newParagraph = document.createElement("p");
            newParagraph.className = "received-gradient";
            newParagraph.textContent = message;
            document.querySelector(".message-container").appendChild(newParagraph);
        }
        function displayMessagePersonal(message) {
            const newParagraph = document.createElement("p");
            newParagraph.className = "sent";
            newParagraph.textContent = message;
            document.querySelector(".message-container").appendChild(newParagraph);
        }

    });
});