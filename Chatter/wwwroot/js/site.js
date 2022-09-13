$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatServer").build();
    connection.start();
    connection.on("Load", function () {
        LoadMessages();
    });

    LoadMessages();


    $('#send').click(function () {
        var msg = $('#text').val();
        var username = $('#username').val();

        var userData = new Object();
        userData.Text = msg;
        userData.Username = username;

        $.ajax({
            url: '/Home/Create',
            type: 'POST',

            data: userData,
            success(data) {
                console.log(data.toString());
                $('#text').val('');
            },
            error(err) {
                console.log(err.toString());
            }

        });

    });

    function LoadMessages() {
        var currentUser = $('#username').val();

        var content = '';
        $.ajax({
            url: '/Home/GetMessages',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    if (currentUser == v.username) {
                        content += `<div class="chat">
                                    <div class="chat-avatar">
                                        <a class="avatar avatar-online" data-toggle="tooltip" href="#" data-placement="right" title="" data-original-title="June Lane">
                                            <img src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="...">
                                            <i></i>
                                        </a>
                                    </div>
                                    <div class="chat-body">
                                        <div class="chat-content">
                                            <p>
                                                ${v.text}
                                            </p>
                                        </div>
                                    </div>
                                </div>`
                    }
                    else {
                        content += `<div class="chat chat-left">
                                    <div class="chat-avatar">
                                        <a class="avatar avatar-online" data-toggle="tooltip" href="#" data-placement="left" title="" data-original-title="Edward Fletcher">
                                            <img src="https://bootdey.com/img/Content/avatar/avatar2.png" alt="...">
                                            <i></i>
                                        </a>
                                    </div>
                                    <div class="chat-body">
                                        <div class="chat-content">
                                    <p>
                                            ${v.text}
                                    </p>
                                        </div>
                                    </div>
                                </div>`
                    }
                })
                $('#chats').html(content);

            },
            error: (error) => {
                console.log(error);
            }
        });

    };

});// End of Documents


