document.getElementById("authorize").addEventListener("click", authorize, false);
document.getElementById("api").addEventListener("click", api, false);

if (localStorage.getItem("access_token") === null) {
    document.getElementById("results").innerHTML += "App unauthorized..." + "\r\n";
} else {
    document.getElementById("results").innerHTML += "Application Authorized!" + "\r\n";
    document.getElementById("results").innerHTML += `<b>access_token:</b> ${localStorage.getItem("access_token")}\r\n`;
    document.getElementById("results").innerHTML += `<b>token_type:</b> ${localStorage.getItem("token_type")}\r\n`;
    document.getElementById("results").innerHTML += `<b>expires_in:</b> ${localStorage.getItem("expires_in")}\r\n`;
    document.getElementById("results").innerHTML += `<b>state:</b> ${localStorage.getItem("state")}\r\n`;
}

function authorize() {
    let random = generateId();
    localStorage.setItem("state", random);

    window.location.replace(`http://localhost:5000/connect/authorize?client_id=implicit_client&scope=wiredbrain_api.rewards&redirect_uri=http://localhost:5004/callback.html&response_type=token&response_mode=fragment&state=${random}`);
}

function api() {
    $.ajax({
        url: "http://localhost:5002/api/rewards",
        type: "GET",
        dataType: "json",
        beforeSend: function (xhr) {
            document.getElementById("results").innerHTML += `\nCalling API with Authorization header: ${localStorage.getItem("token_type")} ${localStorage.getItem("access_token")}\n`;
            xhr.setRequestHeader("Authorization", localStorage.getItem("token_type") + " " + localStorage.getItem("access_token"));
        },
        complete: function (xhr) {
            if (xhr.status === 200) {
                document.getElementById("results").innerHTML += "API access authorized!" + "\n";
            }
            else if (xhr.status === 401) {
                document.getElementById("results").innerHTML += "Unable to contact API: Unauthorized!" + "\n";
            } else {
                document.getElementById("results").innerHTML += "Unable to contact API. Status code " + xhr.status + "\n";
            }
        }
    });
}

// https://stackoverflow.com/a/27747377/3274800
// dec2hex :: Integer -> String
function dec2hex(dec) {
    return (`0${dec.toString(16)}`).substr(-2);
}

// generateId :: Integer -> String
function generateId(len) {
    var arr = new Uint8Array((len || 40) / 2);
    window.crypto.getRandomValues(arr);
    return Array.from(arr, dec2hex).join("");
}