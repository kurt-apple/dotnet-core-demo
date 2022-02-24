const uri = 'api/shoppingitems';
let shoppingList = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addDollarTextbox = document.getElementById('add-dollarcost');
    const addCentTextbox = document.getElementById('add-centcost');
    const addQuantityTextbox = document.getElementById('add-quantity');

    const item = {
        name: addNameTextbox.value.trim(),
        unitCostDollars: parseInt(addDollarTextbox.value.trim()),
        unitCostCents: parseInt(addCentTextbox.value.trim()),
        quantity: parseInt(addQuantityTextbox.value.trim())
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addDollarTextbox.value = 0;
            addCentTextbox.value = 0;
            addQuantityTextbox.value = 1;
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = shoppingList.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Shopping Item' : 'Shopping Items';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayGrandTotal(data) {
    /*let total_cost = 0.0;
    data.forEach(x => {
        total_cost += x.ItemTotalCost;
        console.log("forEach loop: " + x.name + ": " + x.ItemTotalCost);
    });
    document.getElementById('grandtotal').innerText = `Grand Total: $${total_cost}`;*/

    let total_cost = 0.0;

    data.forEach(x => fetch(`{uri}/totalcost`).then(y => total_cost += y))
        .catch(error => console.error('Unable to update item.', error));
}

function _displayItems(data) {
    const tBody = document.getElementById('shopping-list-items');
    tBody.innerHTML = '';

    _displayCount(data.length);
    //_displayGrandTotal(data);

    const button = document.createElement('button');

    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(item.name + ", ");
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let totalCost = document.createTextNode(item.itemTotalCost + ", ");
        td1.appendChild(totalCost);

        let td3 = tr.insertCell(2);
        let qty = document.createTextNode(item.quantity);
        td1.appendChild(qty);

        let td4 = tr.insertCell(3);
        td2.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td3.appendChild(deleteButton);
    });

    shoppingList = data;
}