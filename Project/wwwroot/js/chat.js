﻿const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Hubs/ChatHub").build();
var Dct_id=0;//1
var Ptn_id = 0;//1
var Ptn_name = "";//abdulrehman
var User_role = "";//Doctor
// Function to send a message
function sendMessage(recipientId, message, senderId) {
    connection.invoke("SendMessage", recipientId, message, senderId, User_role).catch(err => console.error("Error sending message:", err));
}
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("btn_send").addEventListener("click", () => {
        const message = document.getElementById("messageInput").value;
        // Create a new p element
        const newParagraph = document.createElement("p");
        newParagraph.className = "sent";
        // Set the content of the new p element to the extracted text
        newParagraph.textContent = message;

        // Append the new p element to the "reverse_chat" div
        document.querySelector(".message-container").appendChild(newParagraph);

        var chatInfo = getChatCookie();
        if (chatInfo) {
            var recipientId = chatInfo.doctor_id;
            Dct_id = recipientId;
            var senderId = chatInfo.patient_id;
            Ptn_id = senderId;
            sendMessage(parseInt(recipientId), message, parseInt(senderId), User_role);
            document.getElementById("messageInput").value = "";
        }
        else {
            document.getElementById("messageInput").value = "Ids not detected";
        }
    });
    connection.on("ReceiveMessageDoctor", (senderId, message) => {
        if (parseInt(senderId) == Dct_id && message.userRole !== User_role) {
            displayMessage(message.messageContent);
        }
        else {
            console.log("User not found");
        }

    });
    // Event listener for receiving messages from the server
    connection.on("LoadChatPatient", (senderId, messages) => {
        console.log(Dct_id, Ptn_id);
        if (parseInt(senderId) == Ptn_id) {
            if (messages === null) {
                console.log("No message in db");
            }
            else
            {
                for (const message of messages) {
                    if (message.userRole === "Patient") {
                        displayMessagePersonal(message.messageContent);
                    }
                    else if (message.userRole === "Doctor") {
                        displayMessage(message.messageContent);
                    }
                }

            }
        }
        else {
            console.log("User not found");
        }
        
    });
    connection.on("LoadChatDoctor", (senderId, messages) => {
        if (parseInt(senderId) == Dct_id) {
            console.log(messages);

            if (messages === null) {
                console.log("No message in db");
            }
            else {
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

    });
    // Start the connection to the SignalR hub
    connection.start().then(() => {
        console.log("Connection started");
        console.log(Dct_id,Ptn_id)
        loadMessages();
    }).catch(err => console.error(err));
    function loadMessages() {
        var chatInfo = getChatCookie();
        if (chatInfo) {
            var recipientId = chatInfo.doctor_id;
            Dct_id = recipientId;
            var senderId = chatInfo.patient_id;
            Ptn_id = senderId;
            Ptn_name = chatInfo.patient_name;
            User_role = chatInfo.user_role;
            console.log(Ptn_name);
            document.getElementById("ptn_name").textContent = Ptn_name;
        }
        else {
            document.getElementById("messageInput").value = "Ids not detected";
        }
        connection.invoke("GetMessages", parseInt(Dct_id), parseInt(Ptn_id), User_role).catch(err => console.error("Error calling get all messages :", err));
    }
    function displayMessage(message) {
        if (message === null)
        {
            console.log("No message to display from ")
        }
        // Create a new p element
        const newParagraph = document.createElement("p");
        newParagraph.className = "received-gradient";
        // Set the content of the new p element to the extracted text
        
        newParagraph.textContent = message;

        // Append the new p element to the "reverse_chat" div
        document.querySelector(".message-container").appendChild(newParagraph);
    }
    function displayMessagePersonal(message) {
        // Create a new p element
        const newParagraph = document.createElement("p");
        newParagraph.className = "sent";
        // Set the content of the new p element to the extracted text
        newParagraph.textContent = message;

        // Append the new p element to the "reverse_chat" div
        document.querySelector(".message-container").appendChild(newParagraph);
    }
    function getChatCookie() {
        var cookieValue = document.cookie
            .split('; ')
            .find(row => row.startsWith('ChatCookie='))
            .split('=')[1];

        if (cookieValue) {
            return JSON.parse(cookieValue);
        } else {
            return null;
        }
    }
});

    