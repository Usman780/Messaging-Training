


/* ------------------------------------------------------------------------------
*
*  # Fullcalendar basic options
*
*  Specific JS code additions for extra_fullcalendar_views.html and 
*  extra_fullcalendar_styling.html pages
*
*  Version: 1.0
*  Latest update: Aug 1, 2015
*
* ---------------------------------------------------------------------------- */
$(document).ready(function () {
    LoadCalendarData();
   // TestAbc();
    function LoadCalendarData() {
        var count = 0;
        $(function () {
            $('.fullcalendar-basic').fullCalendar({

                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                },
                //defaultDate: '12-10-2020',
                defaultDate: new Date(),
                editable: true,
                eventLimit: true, // for all non-TimeGrid views

                events: data,
                //progressiveEventRendering: true,

                viewRender: function (view) {
                    // This does not fire when switching from listMonth (or week or whatever) to month
                    console.log('view', view)
                },

                eventClick: function (event, jsEvent, view) {

                    if (event.IsOutlook == "1") {
                        outlooktask(event.title, "", "");
                    }
                    //$.ajax({
                    //    type: 'POST',
                    //    url: "../task/c",
                    //    data: {
                    //        ids: event.url,
                    //        title: event.title,
                    //        start: event.r,
                    //        DepartmentID: event.DepartmentID,
                    //        DivisionID: event.DivisionID,


                    //    },
                    //    dataType: "json",
                    //    success: function (data) {
                    //        if (data.Id == -121) {
                    //            $("#checkSession").modal("show");
                    //        } else if (data.Id == -99) {
                    //            outlooktask(data.GroupTaskName, data.StartDate, data.EndDate);
                    //        }
                    //        else {

                    //            var x = 1;
                    //            if (data.Id == -88) {
                    //                var origin = window.location.origin;
                    //                var empurl;
                    //                var manurl;

                    //                empurl = "" + origin + "/Task/assignments?DivDepID=" + data.GroupTaskName + "&isModal=01";
                    //                window.location.href = empurl;


                    //            }
                    //            if (data.IsGroupTask == "0") {
                    //                var origin = window.location.origin;
                    //                var empurl;
                    //                var manurl;
                    //                if (data.Role == 3) {
                    //                    empurl = "" + origin + "/Task/taskdetails?v=" + data.EncryptedTaskId + "&isModal=01";
                    //                    window.location.href = empurl;
                    //                } else if (data.Role == 1 || data.Role == 2 || data.Role == 4) {
                    //                    manpurl = "" + origin + "/Manager/taskdetails?v=" + data.EncryptedTaskId + "&isModal=01";

                    //                    window.location.href = manpurl;
                    //                }


                    //                if (data.Employee == 1) {
                    //                    var newUrl = "../Task/taskDetails?v=" + data.Id + "&isModal=" + x;
                    //                } else {
                    //                    var newUrl = "../Manager/taskDetails?v=" + data.Id;
                    //                }


                    //                $('#usertasktodoid').val(data.Id);
                    //                $('#h5taskname').html(data.TaskName);
                    //                $('#pnotes').html(data.Notes);
                    //                $('#completedTicketscountspan').html(data.completedTicketscount);
                    //                $('#uncompletedTicketscountspan').html(data.uncompletedTicketscount);
                    //                $('#taskdetailsid').attr("href", newUrl);
                    //                if (data.StatusValue == "1") {
                    //                    $('#statusone').addClass("active");
                    //                    $("#statusone a").css('color', '#ffffff');
                    //                    $('#statustwo').removeClass("active");
                    //                    $('#statusthree').removeClass("active");
                    //                    $('#statusfour').removeClass("active");
                    //                }
                    //                else if (data.StatusValue == "2") {
                    //                    $('#statustwo').addClass("active");
                    //                    $("#statustwo a").css('color', '#ffffff');
                    //                    $('#statusone').removeClass("active");
                    //                    $('#statusthree').removeClass("active");
                    //                    $('#statusfour').removeClass("active");
                    //                }
                    //                else if (data.StatusValue == "3") {
                    //                    $('#statusthree').addClass("active");
                    //                    $("#statusthree a").css('color', '#ffffff');
                    //                    $('#statustwo').removeClass("active");
                    //                    $('#statusone').removeClass("active");
                    //                    $('#statusfour').removeClass("active");
                    //                }
                    //                else if (data.StatusValue == "4") {
                    //                    $('#statusfour').addClass("active");
                    //                    $("#statusfour a").css('color', '#ffffff');
                    //                    $('#statustwo').removeClass("active");
                    //                    $('#statusthree').removeClass("active");
                    //                    $('#statusone').removeClass("active");
                    //                }

                    //                Active ToDos
                    //                $("#uncompletedTicketsTable > tbody").html("");
                    //                var i = 0;
                    //                var l = 0;
                    //                var $sttr = "";
                    //                $.each(data.uncompletedTickets, function () {

                    //                    $sttr = "";

                    //                    if (~data.uncompletedTickets[i].Name.indexOf(" qqqqaaaa ")) {
                    //                        $sttr = data.uncompletedTickets[i].Name.replace(" qqqqaaaa ", " ");

                    //                    }
                    //                    if ($sttr != "") {
                    //                        var trobject = '<tr><td>' + (i + 1) + '<td><sup><b>' + '(Edited)' + '</b></sup><br/>' + $sttr + '</td><td>' + data.uncompletedTickets[i].CreationDatetime + '</td><td><button class="btn bg-primary-400" onclick="completetodoajax(' + data.uncompletedTickets[i].Id + ')"> Close</button></td >';
                    //                        trobject = trobject + '</tr>';
                    //                    } else {
                    //                        var trobject = '<tr><td>' + (i + 1) + '<td>' + data.uncompletedTickets[i].Name + '</td><td>' + data.uncompletedTickets[i].CreationDatetime + '</td><td><button class="btn bg-primary-400" onclick="completetodoajax(' + data.uncompletedTickets[i].Id + ')"> Close</button></td >';
                    //                        trobject = trobject + '</tr>';
                    //                    }
                    //                    $('#uncompletedTicketsTable > tbody:last-child').append(trobject);
                    //                    i++;
                    //                });


                    //                InActive ToDos
                    //                $("#completedTicketsTable > tbody").html("");
                    //                var j = 0;
                    //                $sttr2 = "";

                    //                $.each(data.completedTickets, function () {

                    //                    $sttr2 = "";

                    //                    if (~data.completedTickets[j].Name.indexOf(" qqqqaaaa ")) {
                    //                        $sttr2 = data.completedTickets[j].Name.replace(" qqqqaaaa ", " ");

                    //                    }
                    //                    if ($sttr2 != "") {
                    //                        var trobject = '<tr><td>' + i + '</td><td>' + $sttr2 + '</td><td>' + data.completedTickets[j].CreationDatetime + '</td >';
                    //                        trobject = trobject + '</tr>';
                    //                    } else {
                    //                        var trobject = '<tr><td>' + i + '</td><td>' + data.completedTickets[j].Name + '</td><td>' + data.completedTickets[j].CreationDatetime + '</td >';
                    //                        trobject = trobject + '</tr>';
                    //                    }


                    //                    $('#completedTicketsTable > tbody:last-child').append(trobject);
                    //                    j++;
                    //                });

                    //                Comments
                    //                $("#commentsUl > li").html("");
                    //                j = 0;
                    //                $.each(data.comments, function () {

                    //                    if (data.comments[j].ROLE != null) {

                    //                        if (data.comments[j].obj[0]) {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.comments[j].Name + '<sup><b>' + data.comments[j].ROLE + '</b></sup></a><span class="media-annotation dotted">' + data.comments[j].CommentTime + '</span></div><p>' + data.comments[j].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box; margin-left:15px"><li><a href="../Manager/taskDetails?v=' + data.Id + '&replyId=' + data.comments[j].Id + '">Reply</a></li><li style="margin-left:20px"><a href="../Manager/taskDetails?v=' + data.Id + '&viewrepliesid=' + data.comments[j].Id + '">View Replies</a></li></ul></li>';

                    //                        } else {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.comments[j].Name + '<sup><b>' + data.comments[j].ROLE + '</b></sup></a><span class="media-annotation dotted">' + data.comments[j].CommentTime + '</span></div><p>' + data.comments[j].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box; margin-left:15px"><li><a href="../Manager/taskDetails?v=' + data.Id + '&replyId=' + data.comments[j].Id + '">Reply</a></li></ul></li>';
                    //                        }

                    //                    } else {
                    //                        if (data.comments[j].obj[0]) {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.comments[j].Name + '</a><span class="media-annotation dotted">' + data.comments[j].CommentTime + '</span></div><p>' + data.comments[j].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box; margin-left:15px"><li><a href="../Manager/taskDetails?v=' + data.Id + '&replyId=' + data.comments[j].Id + '">Reply</a></li><li style="margin-left:20px"><a href="../Manager/taskDetails?v=' + data.Id + '&viewrepliesid=' + data.comments[j].Id + '">View Replies</a></li></ul></li>';

                    //                        } else {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.comments[j].Name + '</a><span class="media-annotation dotted">' + data.comments[j].CommentTime + '</span></div><p>' + data.comments[j].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box; margin-left:15px"><li><a href="../Manager/taskDetails?v=' + data.Id + '&replyId=' + data.comments[j].Id + '">Reply</a></li></ul></li>';
                    //                        }

                    //                    }

                    //                    $('#commentsUl > li:last-child').append(trobject);
                    //                    j++;
                    //                });

                    //                Comment Form
                    //                $("#commentform").html("");

                    //                var trobject = '<h6 style="margin-left:17px" class="text-semibold"><i class="icon-pencil7 position-left"></i> Quick Comment</h6><div class="content-group"><div class="col-md-12"><textarea rows="4" class="form-control" id="taskcomment" name="comment" required></textarea><br /> </div></div><input type="hidden" id="commenttaskId" value="' + data.Id + '" /><div class="text-right"><button id="btnn" onclick="addcommentajax()" class="btn  btn-sm bg-primary-400" ><i class="glyphicon glyphicon-send"></i> Add comment</button ></div>';
                    //                $('#commentform:last-child').append(trobject);

                    //            }

                    //            if (data.IsGroupTask == "1") {
                    //                var origin = window.location.origin;
                    //                grpurl = "" + origin + "/Task/grouptaskdetails?v=" + data.EncryptedTaskId + "&isModal=01";
                    //                window.location.href = grpurl;



                    //                var newUrl = "../Task/grouptaskDetails?v=" + data.Id;
                    //                $('#grouptasktodoid').val(data.Id);
                    //                $('#h5grouptaskname').html(data.GroupTaskName);
                    //                $('#pgroupnotes').html(data.Notes);
                    //                $('#completedgroupTicketscountspan').html(data.completedgroupTicketscount);
                    //                $('#uncompletedgroupTicketscountspan').html(data.uncompletedgroupTicketscount);
                    //                $('#grouptaskdetailsid').attr("href", newUrl);

                    //                if (data.CurrentStatus == "1") {
                    //                    $('#groupstatusone').addClass("active");

                    //                    $('#groupstatustwo').removeClass("active");
                    //                    $('#groupstatusthree').removeClass("active");
                    //                    $('#groupstatusfour').removeClass("active");
                    //                }
                    //                else if (data.CurrentStatus == "2") {
                    //                    $('#groupstatustwo').addClass("active");

                    //                    $('#groupstatusone').removeClass("active");
                    //                    $('#groupstatusthree').removeClass("active");
                    //                    $('#groupstatusfour').removeClass("active");
                    //                }
                    //                else if (data.CurrentStatus == "3") {
                    //                    $('#groupstatusthree').addClass("active");

                    //                    $('#groupstatustwo').removeClass("active");
                    //                    $('#groupstatusone').removeClass("active");
                    //                    $('#groupstatusfour').removeClass("active");
                    //                }
                    //                else if (data.CurrentStatus == "4") {
                    //                    $('#groupstatusfour').addClass("active");

                    //                    $('#groupstatustwo').removeClass("active");
                    //                    $('#groupstatusthree').removeClass("active");
                    //                    $('#groupstatusone').removeClass("active");
                    //                }

                    //                Active ToDos
                    //                $("#uncompletedgroupTicketsTable > tbody").html("");
                    //                var i = 0;
                    //                $sttr3 = "";
                    //                $.each(data.uncompletedgroupTickets, function () {

                    //                    $sttr3 = "";

                    //                    if (~data.uncompletedgroupTickets[i].Name.indexOf(" qqqqaaaa ")) {
                    //                        $sttr3 = data.uncompletedgroupTickets[i].Name.replace(" qqqqaaaa ", " ");

                    //                    }
                    //                    if ($sttr3 != "") {

                    //                        var trobject = '<tr><td>' + (i + 1) + '</td>' + '<td><sup><b>' + '(Edited)' + '</b></sup><br/>' + $sttr3 + '</td><td>' + data.uncompletedgroupTickets[i].CreationDatetime + '</td><td><button class="btn bg-primary-400" onclick="completegrouptodoajax(' + data.uncompletedgroupTickets[i].Id + ')"> Close</button></td >';
                    //                        trobject = trobject + '</tr>';
                    //                    } else {
                    //                        var trobject = '<tr><td>' + (i + 1) + '</td><td>' + data.uncompletedgroupTickets[i].Name + '</td><td>' + data.uncompletedgroupTickets[i].CreationDatetime + '</td><td><button class="btn bg-primary-400" onclick="completegrouptodoajax(' + data.uncompletedgroupTickets[i].Id + ')"> Close</button></td >';
                    //                        trobject = trobject + '</tr>';
                    //                    }



                    //                    $('#uncompletedgroupTicketsTable > tbody:last-child').append(trobject);
                    //                    i++;
                    //                });

                    //                InActive ToDos
                    //                $("#completedgroupTicketsTable > tbody").html("");
                    //                var j = 0;

                    //                $sttr4 = "";
                    //                $.each(data.completedgroupTickets, function () {


                    //                    $sttr4 = "";

                    //                    if (~data.completedgroupTickets[j].Name.indexOf(" qqqqaaaa ")) {
                    //                        $sttr4 = data.completedgroupTickets[j].Name.replace(" qqqqaaaa ", " ");

                    //                    }
                    //                    if ($sttr4 != "") {

                    //                        var trobject = '<tr><td>' + (j + 1) + '</td><td>' + $sttr4 + '</td><td>' + data.completedgroupTickets[j].CompletionDatetime + '</td>';
                    //                        trobject = trobject + '</tr>';
                    //                    } else {
                    //                        var trobject = '<tr><td>' + (j + 1) + '</td><td>' + data.completedgroupTickets[j].Name + '</td><td>' + data.completedgroupTickets[j].CompletionDatetime + '</td>';
                    //                        trobject = trobject + '</tr>';
                    //                    }


                    //                    $('#completedgroupTicketsTable > tbody:last-child').append(trobject);
                    //                    j++;
                    //                });

                    //                Comments
                    //                $("#commentsgroupUl > li").html("");
                    //                k = 0;
                    //                $.each(data.GroupTaskComments, function () {

                    //                    if (data.GroupTaskComments[k].ROLE == null) {
                    //                        if (data.GroupTaskComments[k].obj[0]) {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.GroupTaskComments[k].Name + '</a><span class="media-annotation dotted">' + data.GroupTaskComments[k].CommentTime + '</span></div><p>' + data.GroupTaskComments[k].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box; margin-left:15px"><li><a href="../Task/grouptaskDetails?v=' + data.Id + '&replyId=' + data.GroupTaskComments[k].Id + '">Reply</a></li><li style="margin-left:20px"><a href="../Task/grouptaskDetails?v=' + data.Id + '&viewrepliesid=' + data.GroupTaskComments[k].Id + '">View Replies</a></li></ul></li>';

                    //                        } else {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.GroupTaskComments[k].Name + '</a><span class="media-annotation dotted">' + data.GroupTaskComments[k].CommentTime + '</span></div><p>' + data.GroupTaskComments[k].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box; margin-left:15px"><li><a href="../Task/grouptaskDetails?v=' + data.Id + '&replyId=' + data.GroupTaskComments[k].Id + '">Reply</a></li></ul></li>';


                    //                        }

                    //                    } else {
                    //                        if (data.GroupTaskComments[k].obj[0]) {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.GroupTaskComments[k].Name + '<sup><b>' + data.GroupTaskComments[k].ROLE + '</b></sup></a><span class="media-annotation dotted">' + data.GroupTaskComments[k].CommentTime + '</span></div><p>' + data.GroupTaskComments[k].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box;margin-left:15px"><li><a href="../Task/grouptaskDetails?v=' + data.Id + '&replyId=' + data.GroupTaskComments[k].Id + '">Reply</a></li><li style="margin-left:20px"><a href="../Task/grouptaskDetails?v=' + data.Id + '&viewrepliesid=' + data.GroupTaskComments[k].Id + '">View Replies</a></li></ul></li>';

                    //                        } else {
                    //                            var trobject = '<li class="media"><div class="media-left"><a href="#"><img src="assets/images/placeholder.jpg" class="img-circle img-sm" alt=""></a></div><div class="media-body"><div class="media-heading"> <a href="#" class="text-semibold">'
                    //                                + data.GroupTaskComments[k].Name + '<sup><b>' + data.GroupTaskComments[k].ROLE + '</b></sup></a><span class="media-annotation dotted">' + data.GroupTaskComments[k].CommentTime + '</span></div><p>' + data.GroupTaskComments[k].Comment + '</p></div>';
                    //                            trobject = trobject + '<ul type="none" style="display: -webkit-inline-box;margin-left:15px"><li><a href="../Task/grouptaskDetails?v=' + data.Id + '&replyId=' + data.GroupTaskComments[k].Id + '">Reply</a></li></ul></li>';

                    //                        }

                    //                    }

                    //                    $('#commentsgroupUl > li:last-child').append(trobject);
                    //                    k++;
                    //                });

                    //                Comment Form
                    //                $("#commentgroupform").html("");

                    //                var trobject = '<h6 style="margin-left:17px" class="text-semibold"><i class="icon-pencil7 position-left"></i> Quick Comment</h6><div class="content-group"><div class="col-md-12"><textarea rows="4" class="form-control" id="grouptaskcomment" name="commet" required></textarea><br /> </div></div><input type="hidden" id="commentgrouptaskuserId" value="' + data.grouptaskuserId + '" /><input type="hidden" id="commentgrouptaskdetailId" value="' + data.Id + '" /><div class="text-right"><button id="btnn" onclick="addgroupcommentajax()" class="btn btn-sm bg-primary-400 " ><i class="glyphicon glyphicon-send"></i> Add comment</button ></div>';
                    //                $('#commentgroupform:last-child').append(trobject);
                    //            }

                    //        }
                    //    },
                    //    error: function () { $("#CalendarResponse").modal("show"); }
                    //});

                },

                dayClick: function (date, jsEvent, view) {
                    //var dt = new Date();
                    //var dt2 = new Date(date);
                    //var dtday = dt.getDay();
                    //var dtmonth = dt.getMonth();
                    //var dtyear = dt.getFullYear();

                    //var dt2day = dt2.getDay();
                    //var dt2month = dt2.getMonth();
                    //var dt2year = dt2.getFullYear();
                    //var same = 0;
                    //var Date1 = new Date(dtyear, dtmonth, dtday);
                    //var Date2 = new Date(dt2year, dt2month, dt2day);
                    //if (Date1 < Date2) {
                    //    same = 1;
                    //} else if (Date1 > Date2) {
                    //    same = -1;
                    //} else {
                    //    same = 0;
                    //}

                    $('#TimelineDate').val(date.format('Y-MM-DD HH:mm:ss'));
                    $('#ManagerTimelineDate').val(date.format('Y-MM-DD HH:mm:ss'));
                    $('#EmployeeTimelineDate').val(date.format('Y-MM-DD HH:mm:ss'));

                    $.ajax({
                        type: 'POST',
                        url: "../task/getModalDTOs",
                        dataType: "json",
                        success: function (data) {
                            // get role of the user then if you are an admin then assign tasks to manager and employee
                            // if you are manager then assign to employee
                            // if you can self assign
                            var role = data;

                            if (role == "1") {

                                $("#selectAdminTaskType").val("taskAssignment").change();
                                var selectday = $("#TimelineDate").val();
                                selectday = selectday.split(" ")[0];
                                //  var day = selectday.format("yyyy-MM-dd");
                                $("#StartDate1").val(selectday);

                                // document.getElementById("StartDate1").min = selectday;
                                // document.getElementById("StartDate1").max = selectday;

                                $("#EndDate1").val(selectday);

                                //document.getElementById("EndDate1").min = selectday;
                                // document.getElementById("EndDate1").max = selectday;

                                $('#adminAssignTask').modal();

                            }
                            else if (role == "2" || role == "4") {

                                var selectday = $("#ManagerTimelineDate").val();
                                selectday = selectday.split(" ")[0];
                                //  var day = selectday.format("yyyy-MM-dd");
                                $("#StartDate2").val(selectday);

                                //document.getElementById("StartDate2").min = selectday;
                                //document.getElementById("StartDate2").max = selectday;

                                $("#EndDate2").val(selectday);

                                // document.getElementById("EndDate2").min = selectday;
                                //document.getElementById("EndDate2").max = selectday;

                                $('#managerAssignTask').modal();
                            }
                            else if (role == "3") {

                                var selectday = $("#EmployeeTimelineDate").val();
                                selectday = selectday.split(" ")[0];
                                //  var day = selectday.format("yyyy-MM-dd");
                                $("#StartDate3").val(selectday);

                                //document.getElementById("StartDate3").min = selectday;
                                //document.getElementById("StartDate3").max = selectday;

                                $("#EndDate3").val(selectday);

                                //document.getElementById("EndDate3").min = selectday;
                                // document.getElementById("EndDate3").max = selectday;


                                $('#employeeAssignTask').modal();

                                $.ajax({
                                    type: 'POST',
                                    url: "../Trainee/EmployeeDepartmentTask",
                                    dataType: "json",
                                    success: function (data) {
                                        $("#selectEmployeeTask").html("");
                                        var count = 0;
                                        $('#selectEmployeeTask').empty();
                                        $('#selectEmployeeTask').append('<option value="" selected disabled> Please Choose a Task</option>');
                                        $('#selectEmployeeTask').append('<option value="0"> Create New Task</option>');
                                        $('#selectEmployeeTask').append('<option value="" disabled>Existing Tasks</option>');
                                        for (var i = 0; i < data.length; i++) {
                                            var division = '<option value="' + data[i]['Id'] + '">' + data[i]['Name'] + '</option>';
                                            $('#selectEmployeeTask').append(division);
                                        }
                                    },
                                    error: function () { alert('fail'); }
                                });

                            }
                            else {
                                $("#checkSession").modal("show");
                            }
                        },
                        error: function () { $("#CalendarResponse").modal("show"); }
                    });


                },

                eventDrop: function (event) {
                    $('.popover.in').remove();
                    //alert(event.title + " and on starting date " + event.start.toISOString() + " and ending date " + event.end.toISOString());
                    //var url = event.url;
                    var url = event.OldURL;
                    var start = event.start.format('Y-MM-DD HH:mm:ss');
                    var end;
                    if (event.end == null) {
                        end = "NULL";
                    } else {
                        end = event.end.format('Y-MM-DD HH:mm:ss');
                    }


                    $.ajax({
                        type: 'POST',
                        url: "../task/DragTask",
                        data: {
                            ids: url,
                            start: start,
                            end: end
                        },
                        dataType: "json",
                        success: function (data) {
                            //
                        },
                        error: function () { $("#CalendarResponse").modal("show"); }
                    });

                },

                eventResize: function (event) {
                    $('.popover.in').remove();
                    //var url = event.url;
                    var url = event.OldURL;
                    var end = event.end.format('Y-MM-DD HH:mm:ss');

                    $.ajax({
                        type: 'POST',
                        url: "../task/ResizeTask",
                        data: {
                            ids: url,
                            end: end
                        },

                        dataType: "json",
                        success: function (data) {
                            //
                        },
                        error: function () { $("#CalendarResponse").modal("show"); }
                    });
                },

                eventOrder: "-TaskPriority", //ordering tasks on the basis of Priority

                views: //show number of custom tasks on calender
                {
                    month: //applied on monthly view
                    {
                        eventLimit: 5 // it will show 4 tasks and then 5th will be in "+ more" option
                    },
                    week: //applied on weekly view
                    {
                        eventLimit: 5
                    },
                    day: //applied on daily view
                    {
                        eventLimit: 5
                    }
                },

                eventLimitText: "View All", //change "+ more" with custom text

                //eventRender: function (eventObj, $el) {
                //    $el.popover({
                //        title: eventObj.description,
                //        //content: 'Task Name: ' + eventObj.title + ', Start Date: ' + eventObj.start.format('Y-MM-DD HH:mm:ss') + ', End Date: ' + eventObj.end.format('Y-MM-DD HH:mm:ss'),
                //        content: 'Task Name: ' + eventObj.title + ', Start Date: ' + eventObj.start.format('Y-MM-DD HH:mm:ss'),
                //        trigger: 'hover',
                //        placement: 'top',
                //        container: 'body'
                //    });
                //}

                eventRender: function (eventObj, $el) {

                    //   $el.find(".closeon").on('click', function () {
                    //$el.find('.fc-title').prepend = "Haider";

                    //   });
                    //$el.find('.fc-title').prepend("<i class='icon-menu7'></i>"); 
                    if (eventObj.PrimaryLead == "") {
                        $el.find('.fc-title').prepend("<i class='icon-user'></i> ");
                    }
                    else if (eventObj.PrimaryLead == null) {
                        $el.find('.fc-title').prepend("<i class='icon-task'> </i> ");

                    }
                    else {
                        $el.find('.fc-title').prepend("<i class='icon-users'></i> ");
                    }

                    if (eventObj.editable == true) {
                        $el.bind('contextmenu', function ()  //triggered when right clicked on calender task
                        {

                            if (eventObj.TaskType == 1 || eventObj.TaskType == 2) {
                                $("#updatestatustaskName").text(eventObj.title);
                                $("#updatestatustaskId").val(eventObj.TaskId);
                                $("#updatestatustaskType").val(eventObj.TaskType);

                                //$("#updatestatustaskStatus").val(eventObj.TaskStatus).attr('selected', 'selected');
                                $("#updatestatustaskStatus").val(eventObj.TaskStatus).change();

                                $('#rightOptionModal').modal('show');

                                return false;
                            }
                            else if (eventObj.TaskType == 3 || eventObj.TaskType == 4) {
                                $('#rightOptionModalError').modal('show');

                                return false;

                            }
                        });


                    }


                    $el.popover({
                        title: eventObj.description,
                        //content: 'Task Name: ' + eventObj.title + ', Start Date: ' + eventObj.start.format('Y-MM-DD HH:mm:ss') + ', End Date: ' + eventObj.end.format('Y-MM-DD HH:mm:ss'),
                        content: function () {
                            if (eventObj.PrimaryLead == "") {
                                return '<h6 class="text-center text-bold">' + eventObj.title + '</h6>' + eventObj.EDate + eventObj.TReminder + eventObj.DReminder;
                            }
                            else if (eventObj.PrimaryLead == null) {
                                return '<h6 class="text-center text-bold">Outlook Event: ' + eventObj.title;

                            }
                            else {
                                return '<h6 class="text-center text-bold">' + eventObj.title + '</h6>' + eventObj.EDate + '<br> <b>Primary Lead: </b>' + eventObj.PrimaryLead + '<br> <b>Secondary Lead: </b>' + eventObj.SecondaryLead + eventObj.GtReminder;
                            }
                        },
                        position: {
                            corner: {
                                target: 'center',
                                tooltip: 'bottomMiddle'
                            }
                        },
                        trigger: 'hover',
                        placement: 'top',
                        container: 'body',
                        html: 'true'
                    });
                },

                //eventRightclick: function (event) //triggered when right clicked on calender task
                //{
                //    alert(event.TaskId);

                //    return false; // Prevent browser context menu:
                //}

            });


            function data(start, end, timezone, callback) {


                var today = start["_d"];
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!
                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }
                var x = mm + "/" + dd + "/" + yyyy;

                var end = end["_d"];
                var dd2 = end.getDate();
                var mm2 = end.getMonth() + 1; //January is 0!
                var yyyy2 = end.getFullYear();
                if (dd2 < 10) {
                    dd2 = '0' + dd2
                }
                if (mm2 < 10) {
                    mm2 = '0' + mm2
                }
                var x2 = mm2 + "/" + dd2 + "/" + yyyy2;

                $("#panelheading").css("height", "140px");
                $("#wait").css("display", "block");
                $.ajax({
                    type: 'Post',
                    url: "../task/getTaskCalendars",
                    data: { Start: x, End: x2 },
                    success: function (data) {
                        if (count == 0) {
                            duetask();
                            count = 1;
                        } else {
                            $("#wait").css("display", "none");
                            $("#panelheading").css("height", "100px");
                        }

                        callback(data)
                    },
                    error: function () { $("#CalendarResponse2").modal("show"); }
                });
            }

            function onResize() {
                if (window.innerWidth <= 768) {
                    $('#calendar').fullCalendar('changeView', 'listMonth')
                } else {
                    $('#calendar').fullCalendar('changeView', 'month')
                }
            }

            $(window).on('resize', onResize);
            onResize();
        });
    }

    function TestAbc() {
        LoadCalendarData();
    }

});


$(function loadData(data) {

    // Initialization
    // ------------------------------

    // Basic view
    $('.fullcalendar-basic').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay'
        },
        defaultDate: '2014-11-12',
        editable: true,
        events: events
    });


    // Event colors
    $('.fullcalendar-event-colors').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultDate: '2014-11-12',
        editable: true
    });


    // Event background colors
    $('.fullcalendar-background-colors').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultDate: '2014-11-12',
        editable: true
    });

});
