
function setMail(id, index)
{
    setBg(index);
    const formData = new FormData();
    formData.append("mailId", id);
    const mailInput = document.querySelector("#mailIdInput");
    mailInput.value = id;
    const active_mail = document.querySelector("#active_mail");
    $.ajax({
        type: "POST",
        url: "/admin/GetMailWithResponses",
        data: formData,
        contentType: false,
        processData: false,
        success: (response) => {
            let responseHour = new Date(Date.parse(response["supportForm"]["createDate"]));
            let hour = (responseHour.getHours() < 10) ? `0${responseHour.getHours()}` : `${responseHour.getHours()}`;
            let minute = (responseHour.getMinutes() < 10) ? `0${responseHour.getMinutes()}` : `${responseHour.getMinutes()}`;
            active_mail.innerHTML = `
            <div class="d-flex">
                <h3>${response["supportForm"]["subject"]}</h3>
                <div class="col" style="text-align:right">
                    <span style="font-weight:600">Gönderim Saati: ${hour}:${minute}</span>
                </div>
            </div>
            <div class="d-flex border-bottom">
                <h6 style="font-weight:700">Gönderen: ${response["supportForm"]["name"]}</h6>
                <div class="col" style="text-align:right">
                    <h6 style="font-weight:700">E-Posta: ${response["supportForm"]["email"]}</h6>
                </div>
            </div>
            <div class="mail-content border-bottom" style="margin-top:1rem;padding:.35rem 0">${response["supportForm"]["messsage"]}</div>`;
            if (response["supportFormResponses"].length < 1) { return; }
            active_mail.innerHTML += "<h3 style='text-align:center;margin-top:2.5rem;padding-bottom:.5rem;' class='border-bottom'>Yanıtlarınız</h3>";
            for (let i = 0; i < response["supportFormResponses"].length; i++)
            {
                let responseHour = new Date(Date.parse(response["supportFormResponses"][i]["createDate"]));
                let hour = (responseHour.getHours() < 10) ? `0${responseHour.getHours()}` : `${responseHour.getHours()}`;
                let minute = (responseHour.getMinutes() < 10) ? `0${responseHour.getMinutes()}` : `${responseHour.getMinutes()}`;
                active_mail.innerHTML += `
                    <div class="d-flex mt-3">
                        <h5>${response["supportFormResponses"][i]["subject"]}</h5>
                        <div class="col" style="text-align:right">
                            <span style="font-weight:600">Gönderim Saati: ${hour}:${minute}</span>
                        </div>
                    </div>
                    <div class="d-flex border-bottom>
                        <h6 style="font-weight:700">Gönderen: Siz</h6>
                        <div class="col" style="text-align:right">
                            <h6 style="font-weight:700">E-Posta: Arite</h6>
                        </div>
                    </div>
                    <div class="mail-content border-bottom" style="margin-top:1rem">${response["supportFormResponses"][i]["message"]}</div>`;
            }
        }
    });


}

function setBg(index)
{
    const mails = document.querySelectorAll(".mail");
    for (let i = 0; i < mails.length; i++)
    {
        if (i == index)
        {
            mails[i].classList.add("f1bg");
        }
        else
        {
            mails[i].classList.remove("f1bg");
        }
    }
}