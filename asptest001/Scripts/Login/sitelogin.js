

$(document).ready(function () {
    var notify = undefined;
    var loop = 0;
    
    window.addEventListener('beforeunload', function(event){
        //event.returnValue = 'Are you sure you want to leave?';
    });
    window.onscroll = function () {
        var top = window.pageYOffset || document.documentElement.scrollTop;
       

        if (top > 50) {
            //console.log(document.querySelector('.nav'));
            document.getElementsByClassName('navbar')[0].classList.add("navbar-scroll");
        } else {
            document.getElementsByClassName('navbar')[0].classList.remove("navbar-scroll");
        }
    };
});

//$("#loginForms").onsubmit( function(){
//   alert("test"); 
//});


function submit_form() {
    
    //notify = set_notify();

    var uid = $("input[name='uid']").val();
    var pas = $.md5($("input[name='pass']").val());
     //$.notify('<strong>Loging</strong> :Checking username and password. Please wait....' );

    if (uid == "" || pas == "") {
        notify = set_notify('<strong>Loging</strong> :Checking username and password. Please wait....','danger');
        //notify.update({ 'type': 'danger', 'message': '<strong>Fail</strong> :Your login failed. Please check your username or password.' })
        $("input[name='uid']").attr("style", "border:2px solid red");
        $("input[name='pass']").attr("style", "border:2px solid red");
    } else {
        //$("input[name='pass']").val(pas);
        //console.log($("input[name='pass']").val());
        notify = set_notify('<strong>Loging</strong> :Checking username and password. Please wait....', 'info');
        $("#loginForm").submit();
        //return 0;
    }

    
    }


    function set_login(data) {
        //Fill div with results
        $("*").html(data);
        if ($("val").html() !== undefined) {
            notify.update({ 'type': 'success', 'message': '<strong>Success</strong> :Your Login successful...' });
            $("#member-login").html('');
            $("#member-login").append("<li>" + $("val").html() + "</li>");
            $("#member-login").append("<li><button type='submit' class='btn btn-logout'>Logout</button></li>");
            $("#member-login").removeClass("nav-login").addClass("nav-logout");
            $("#loginForm").attr("action", $("val").attr("link"));
            $("#loginForm").attr("onsubmit", "alert('Logout nakeub!');");

            //set_table();
        }
        else {
            notify.update({ 'type': 'danger', 'message': '<strong>Fail</strong> :Your login failed. Please check your username or password.' })
            $("input[name='uid']").attr("style",  "border:2px solid red");
            $("input[name='pass']").attr("style", "border:2px solid red");
            console.log(data);
        }
    }
    
    function set_table() {
        $.ajax({
            url: $("val").attr("host") + "/Home/Get_table",
            type: "POST",
            dataType: "html",
            //data: { uid: uid, pass: pas },
            cache: false,
            success: function (data) {
                $(".root").html(data);

                $('#example').DataTable({
                    "scrollX": true
                });
                //console.log(data);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }

    function set_notify( mes,type ) { 
        return $.notify( mes
                , {
                  allow_dismiss: false
                , type: type
                , placement: {
                    from: "top",
                    align: "right"
                }
                , offset: {
                    x: 220,
                    y: 85
                }
                , delay: 1000
                , spacing: 20
                , newest_on_top: true
                , animate: {
                        enter: 'animated lightSpeedIn',
                        exit: 'animated lightSpeedOut'
                    }
                , template:
                '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="aalert">' +
                    '<span data-notify="title">{1}</span>' +
                    '<span data-notify="message">{2}</span>' +
                '</div>' 
            });  
    }