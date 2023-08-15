const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Hubs/ChatHub").build();

// Function to send a message
function sendMessage(recipientId, message, senderId) {
    console.log(senderId);
    connection.invoke("SendMessage", recipientId, message, senderId).catch(err => console.error("Error sending message:", err));
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
            var senderId = chatInfo.patient_id;
            sendMessage(parseInt(recipientId), message, parseInt(senderId));
            document.getElementById("messageInput").value = "";
        }
        else {
            document.getElementById("messageInput").value = "Ids not detected";
        }
    });
    // Event listener for receiving messages from the server
    connection.on("ReceiveMessage", (senderId, message) => {
        if (parseInt(senderId) == 0) {
            console.log(message);
            displayMessage(message);
        }
        else {
            console.log("User not found");
        }
        
    });

    // Start the connection to the SignalR hub
    connection.start().then(() => {
        console.log("Connection started");
    }).catch(err => console.error(err));

    function displayMessage(message) {
        // Create a new p element
        const newParagraph = document.createElement("p");
        newParagraph.className = "received-gradient";
        // Set the content of the new p element to the extracted text
        
        newParagraph.textContent = message;

        // Append the new p element to the "reverse_chat" div
        document.querySelector(".message-container").appendChild(newParagraph);
    }
});
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
    