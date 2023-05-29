const viewModel = {}// open AddOrUpdate Team Modal
$("#openAddTeamModal").on("click", function () {
    $("#teamName").val("");
    $(".modal").modal('show')
});

// open AddOrUpdate Team Modal
function onClickUpdateTeam(el, id) {
    const team = $(el).closest('tr').find('td:first').text().trim();
    viewModel.id = id;
    $(".modal").modal('show');
    $("#teamName").val(team);
}

$("#btnAddUpdateTeam").click(function (e) {
    e.preventDefault();
    const teamName = $("#teamName").val();

    // addOrUpdateTeam
    addOrUpdateTeam(teamName, viewModel.id);
});

function onClickDeleteTeam(el, id) {
    $.ajax({
        url: "Team/DeleteTeam",
        type: "DELETE",
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ id: id }),
        success: function () {
            $(el).closest('tr').remove();
            toastr.success(`Takım başarıyla silindi.`);
        },
        error: function () {
            toastr.error("İşlem başarısız oldu.");
        },
        complete: function () {
            $(".modal").modal('hide')
        }
    })
}

function addOrUpdateTeam(teamName, id) {
    //validate inputs
    var formData = new FormData();
    if (validate(teamName)) {
        if (!!id) {
            formData.append('id', id);
            formData.append('teamName', teamName);
            formData.append('logo', $('#logo')[0].files[0]);
        }
        else {
            formData.append('teamName', teamName);
            formData.append('logo', $('#logo')[0].files[0]);
        }
    }

    $.ajax({
        url: "Team/AddOrUpdateTeam",
        type: "POST",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (response) {
            updateUI(teamName, id, response);
            toastr.success(`${teamName} başarıyla eklendi.`);
        },
        error: function () {
            toastr.error("İşlem başarısız oldu.");
        },
        complete: function () {
            $(".modal").modal('hide')
        }
    })
}


function updateUI(teamName, id, response) {
    if (!!id) {
        $(`#${id}`).text(teamName);

        window.location.reload();


    } else {
        const $latestRow = $('tr:last');
        var lastRowSpanValue = $latestRow.find('span').text();

        if (lastRowSpanValue >= 1) {
            ++lastRowSpanValue;
        } else {
            lastRowSpanValue = 1;
        }

        const tr = `<tr>
                    <th scope="row">
                        <div class="media align-items-center">
                            <div class="media-body">
                                <span class="mb-0 text-sm">${lastRowSpanValue}</span>
                            </div>
                        </div>
                    </th>
                    <td>
                        ${teamName}
                    </td>
                    <td>
                        <div class="dropdown"> 
                            <img id='logo-${response.itemGuid}' src='${response.filePath}' class="img-fluid" />
                        </div>
                    </td>
                    <td class="text-right">
                        <button id="btnUpdateTeam" class="btn btn-primary" onclick="onClickUpdateTeam(this,'${response.itemGuid}')">
                            <i class="fa fa-pen" aria-hidden="true"></i>
                        </button>
                        <button class="btn btn-danger" onclick="onClickDeleteTeam(this,'${response.itemGuid}')">
                            <i class="fa fa-trash" aria-hidden="true"></i>
                        </button>
                    </td>
                </tr>`;
        $latestRow.after(tr);
    }
}


function validate(teamName) {
    if (!isNaN(teamName) && teamName.length < 3) {
        $("#emailvalid").show();
        return false;
    }
    else {
        $("#emailvalid").hide();
        return true;
    }
}

function preview() {
    frame.src = URL.createObjectURL(event.target.files[0]);
}

function clearImage() {
    document.getElementById('formFile').value = null;
    frame.src = "";
}

$(document).ready(function () {
   
});