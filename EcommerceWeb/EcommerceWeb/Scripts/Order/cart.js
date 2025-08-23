function increaseQty(id) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var { qty, price } = JSON.parse(this.response);
            let currentTotalPrice = document.getElementById("TotalPrice").innerText;
            let updatedTotalPrice = parseInt(currentTotalPrice)  + price;
            document.getElementById("TotalPrice").innerText = updatedTotalPrice
            updateQty(qty,updatedTotalPrice,id)
        }
    };
    xhttp.open("Post", "/Order/IncreaseCartQty/", true);
    xhttp.setRequestHeader("Content-Type", "application/json");
    xhttp.send(JSON.stringify({ id: id }));

}

function decreaseQty(id) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var { qty, price } = JSON.parse(this.response);
            let currentTotalPrice = document.getElementById("TotalPrice").innerText;
            let updatedTotalPrice = parseInt(currentTotalPrice) -  price;
            document.getElementById("TotalPrice").innerText = updatedTotalPrice
            updateQty(qty, updatedTotalPrice,id);

        }
    };
    xhttp.open("Post", "/Order/DecreaseCartQty/", true);
    xhttp.setRequestHeader("Content-Type", "application/json");
    xhttp.send(JSON.stringify({ id: id }));
}

function updateQty(qty, updatedTotalPrice,id) {
    document.getElementById(`Qty-${id}`).innerHTML = qty;  

    if (updatedTotalPrice === 0) {
        document.getElementById("BtnPlaceOrder").disabled = true;
    }
    else {
        document.getElementById("BtnPlaceOrder").disabled = false;
    }

}
