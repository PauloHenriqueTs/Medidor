"use strict";

jQuery(document).ready(function ($) {
    $("#signalr").hide();
    var value =  $("#jwt").val();
    console.log(value);
    var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/chathub",
        { accessTokenFactory: () => value, EnableCrossDomain = true  }
    ).build();

    connection.on("ReceiveMessage", function (message) {
        console.log(message)
    });

    connection.start().then(function () {
        onnection.invoke("JoinGroup").catch(function (err) {
            return console.error(err.toString());
        });
    }).catch(function (err) {
        return console.error(err.toString());
    });
});