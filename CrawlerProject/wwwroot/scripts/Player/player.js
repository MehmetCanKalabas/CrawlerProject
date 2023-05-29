
const viewModel = {};
$('#openAddPlayerModal').click(function () {
    $("#addPlayerModal").modal('show');


});

function onClickUpdatePlayer(el, id) {
    $("#updatePlayerModal").modal('show');
    viewModel.id = id;
    $.ajax({
        url: `/Player/GetPlayer?id=${id}`,
        type: "GET",
        success: function (response) {
            $('#up_firstName').val(response.name);
            $('#up_lastName').val(response.lastName);
            $('#up_birthday').val(response.birthDay);

            $('#up_teamId').val(response.teamInfo.teamId);
            console.log(response)
        },
        error: function (err) {

        }
    });
}

$("#saveChangesUpdatePlayer").click(function () {
    const data = {
        playerId: viewModel.id,
        playerName: $('#up_firstName').val(),
        lastName: $("#up_lastName").val(),
        birthDay: $("#up_birthday").val(),
        teamId: $("#up_teamId").val()
    };
    $.ajax({
        url: `/Player/UpdatePlayer`,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (response) {
            window.location.reload();
            
            toastr.success("İşlem basarili oldu.");
        },
        error: function (err) {
            toastr.error("İşlem başarısız oldu.");
        },
        complete: function () {
            $("#updatePlayerModal").modal('hide')
        }
    });
});

$('#btnAddPlayer').click(function (e) {
    let payload = {
        playerName: $("#firstName").val(),
        lastName: $('#lastName').val(),
        birthDay: $('#birthday').val(),
        teamId: $("#teamId").val(),

    };

    $.ajax({
        url: "AddPlayer",
        type: "POST",
        data: JSON.stringify(payload),
        contentType: 'application/json',
        success: function (response) {
            updateUI(payload, response);
            toastr.success(`oyuncu başarıyla eklendi.`);
        },
        error: function () {
            toastr.error("İşlem başarısız oldu.");
        },
        complete: function () {
            $(".modal").modal('hide')
        }
    });

})


function updateUI(data, response) {
    debugger
    console.log(response);
    const teamName = $("#teamId").text().trim();
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
                    <td>das dasasd</td>
                    <td>
                        <div class="birth-date">
                            <div class="birth-date__full">${data.birthDay}</div>
                            <div class="birth-date__age">(${data.birthDay})</div>
                        </div>
                    </td>

                    <td>
                        ${teamName}
                    </td>
                    <td class="text-right">
                        <button id="btnUpdatePlayer" class="btn btn-primary" onclick="onClickUpdatePlayer('${response.id}')">
                            <i class="fa fa-pen" aria-hidden="true"></i>
                        </button>
                        <button class="btn btn-danger" onclick="onClickDeletePlayer(this,'${response.id}')">
                            <i class="fa fa-trash" aria-hidden="true"></i>
                        </button>
                    </td>
                </tr>`;
    $latestRow.after(tr);

}

function onClickDeletePlayer(el, id) {
    $.ajax({
        url: "DeletePlayer",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ id: id }),
        success: function () {
            $(el).closest('tr').remove();
        },
        error: function () {

            toastr.error("İşlem başarısız oldu.");
        },
        complete: function () {
            $(".modal").modal('hide');
        }
    });
}




$(document).ready(function () {
    console.log("Player.js loaded");
});