const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Hubs/ChatHub").build();

    // Function to send a message
    function sendMessage(recipientId, message, senderId) {
        connection.invoke("SendMessage", recipientId, message, senderId).catch(err => console.error(err));
    }
    // Event listener for the send button
    document.getElementById("btn_send").addEventListener("click", () => {
            const message = document.getElementById("messageInput").value;
            var chatInfo = getChatCookie();
        if (chatInfo) {
            var recipientId = chatInfo.doctor_id;
            var senderId = chatInfo.patient_id;
            sendMessage(recipientId, message, senderId);
            document.getElementById("messageInput").value = "";
        }
        else {
            document.getElementById("messageInput").value = "Ids not detected";
        }
    });

    // Function to extract the doctor ID from the "doctor" cookie
    function getDoctorIdFromCookie() {
        const doctorCookie = getCookie("doctor");
        if (doctorCookie) {
            return parseInt(doctorCookie);
        }
        return null;
    }
    // Function to extract the patient ID from the "logedin_user" cookie
    function getPatientIdFromCookie() {
        const patientCookie = getCookie("logedin_user");
        if (patientCookie) {
            return parseInt(patientCookie);
        }
        return null;
    }

    // Helper function to get the value of a cookie by name
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            return parts.pop().split(";").shift();
        }
        return null;
    }

    // Event listener for receiving messages from the server
    connection.on("ReceiveMessage", (message) => {
        // Display the received message
        displayMessage(message);
    });

    // Start the connection to the SignalR hub
    connection.start().then(() => {
        console.log("Connection started");
    }).catch(err => console.error(err));

