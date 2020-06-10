$(document).ready(function () {
    MemberEligibilityListBinding();
    GetAllTechnologies();
    $("#txtDOB").datepicker();
    validationsForDecimal();
});
function validationsForDecimal() {
    $("#txtYearsOfExperience").keypress(function (e) {
        if (e.which == 46) {
            if ($(this).val().indexOf('.') != -1) {
                return false;
            }
        }

        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}

function AddNewMember() {
    $("#myModel").modal("show");
    resetAllProperties();
};

function MemberEligibilityListBinding() {
    var $url = rootURL + "api/MemberEligibility/GetAllMemberDetails";
    $.ajax({
        type: "GET",
        url: $url,
        contentType: false,
        processData: false,
        success: function (result) {

            var trHTML = '';
            $.each(result, function (i, item) {
                trHTML += "<tr>";
                trHTML += "<td>" + item.MemberName + "</td>";
                trHTML += "<td>" + item.DOB + "</td>";
                trHTML += "<td>" + item.Qualification + "</td>";
                trHTML += "<td>" + item.YearsOfExperience + "</td>";
                trHTML += "<td><div class='row'>";
                trHTML += "<a src='#' onclick='GetMemberRecordByID(" + item.MemberID + ")' id='aEdit" + item.MemberID + "' name='aEdit" + item.MemberID + "' class='clsEdit" + item.MemberID + "' style='cursor: default;'>EDIT </a>"
                trHTML += "<a src='#' onclick='DeleteMemberRecordByID(" + item.MemberID + ")' id='aDelete" + item.MemberID + "' name='aDelete" + item.MemberID + "' class='clsDelete" + item.MemberID + "' style='cursor: default;'  > Delete </a>"
                trHTML += "</td></div>"
                trHTML += "</tr>";
            });
            $('#tblCustomer').append(trHTML);
        },
        error: function () {

        }
    });
}

function GetAllTechnologies() {
    var $url = rootURL + "api/MemberEligibility/GetAllTechnologies";
    $.ajax({
        type: "GET",
        url: $url,
        contentType: false,
        processData: false,
        success: function (result) {
            var $appendTechnologies = "";
            $("#ddlTechnologies").empty();
            $("#ddlTechnologies").append($("<option/>").val('-1').html('Select Technologies'));
            $.each(result, function (index, value) {
                $("#ddlTechnologies").append($("<option></option>").val(value.TechnologyID).text(value.TechnologyName));
            });
        },
        error: function () {

        }
    });
}
var frmMemberValidations = null;
function checkValidations() {
    frmMemberValidations = $("#frmMember").validate({
        ignore: "",
        focusInvalid: true,
        submitHandler: function (form) {
            SaveChanges();
        },
        rules: {
            txtMName: {
                required: true
            },
            txtQualification: {
                required: true
            },
            txtYearsOfExperience: {
                required: true,
            },
            txtDOB: {
                required: true
            },
            ddlTechnologies: {
                required: true,
                Technologies: true
            }
        }, messages: {
            txtMName: {
                required: "Enter the member name"
            },
            txtQualification: {
                required: "Enter qualification"
            },
            txtYearsOfExperience: {
                number: "Enter years of experience",
            },
            txtDOB: {
                required: "Enter the date of birth"
            },
            ddlTechnologies: {
                required: "Select technologies",
                Technologies: "Select technologies"
            },
        },
        errorClass: 'help-block',
    });

    $.validator.addMethod("Technologies", function (value, element) {
        return value != null && value != "" && value != -1
    }, "Select technologies");
}

function SaveMethod() {
    checkValidations();
    $("#frmMember").validate();
    if ($("#frmMember").valid()) {
        SaveChanges();
    }
}
function SaveChanges() {
    var $hddnId = $("#hddnId").val();
    var $txtMName = $("#txtMName").val();
    var $txtQualification = $("#txtQualification").val();
    var $txtYearsOfExperience = Number($("#txtYearsOfExperience").val());
    var value = $("#ddlTechnologies option:selected").val();
    var $txtDOB = $("#txtDOB").val();

    if ($txtDOB != null) {
        var now = new Date();
        var past = new Date($txtDOB);
        var nowYear = now.getFullYear();
        var pastYear = past.getFullYear();
        var age = nowYear - pastYear;

        if (age < 25) {
            alert("Date of birth greater than or equal to 25 years is valid.");
            return false;
        }
    }
    if ($txtYearsOfExperience < 3) {
        alert("Minimum 3 years of experience valid.");
        return false;
    }

    var MemberEntityModel = {
        MemberID: $hddnId,
        TechnologyID: value,
        MemberName: $txtMName,
        DateOfBirth: $txtDOB,
        Qualification: $txtQualification,
        YearsOfExperience: $txtYearsOfExperience
    };

    var $url = rootURL + "api/MemberEligibility/SaveMemberDetails";
    $.ajax({
        type: "POST",
        url: $url,
        data: JSON.stringify(MemberEntityModel),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        processData: false,
        success: function (result) {
            if (result.Valid) {
                $("#myModel").modal("hide");
                resetAllProperties();
                $("#frmMember tr").detach();
                MemberEligibilityListBinding();
                alert(result.Message);
            } else {
                $("#myModel").modal("show");
                alert(result.Message);
            }
        },
        error: function () {

        }
    });
}


function GetMemberRecordByID(memberID) {
    resetAllProperties();
    var $url = rootURL + "api/MemberEligibility/MemberDetails?memberID=" + memberID;
    $.ajax({
        type: "GET",
        url: $url,
        contentType: false,
        processData: false,
        success: function (response) {
            var $result = $.parseJSON(response);
            $("#hddnId").val($result.MemberID);
            $("#txtMName").val($result.MemberName);
            $("#txtQualification").val($result.Qualification);
            $("#txtYearsOfExperience").val($result.YearsOfExperience);
            $("#txtDOB").val($result.DateOfBirth);
            $("#ddlTechnologies").val($result.TechnologyID);
            if ($result.DateOfBirth != null) {
                $('#txtDOB').datepicker("setDate", new Date($result.DateOfBirth))
            } else {
                $('#txtDOB').datepicker("setDate", null);
            }
            $("#myModel").modal("show");
        },
        error: function () {

        }
    });
}

function DeleteMemberRecordByID(memberID) {
    var $url = rootURL + "api/MemberEligibility/DeleteMemberDetails?memberID=" + memberID;
    $.ajax({
        type: "GET",
        url: $url,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.Valid) {
                alert(result.Message);
                MemberEligibilityListBinding();
            } else {
                alert(result.Message);
            }
        },
        error: function () {

        }
    });
}

function resetAllProperties() {
    $("#txtMName-error").text('');
    $("#txtDOB-error").text('');
    $("#txtQualification-error").text('');
    $("#ddlTechnologies-error").text('');
    $("#txtYearsOfExperience-error").text('');

    $("#ddlTechnologies").removeClass('help-block');
    $("#txtMName").removeClass('help-block');
    $("#txtDOB").removeClass('help-block');
    $("#txtQualification").removeClass('help-block');
    $("#txtYearsOfExperience").removeClass('help-block');

    $("#frmMember").get(0).reset();
}