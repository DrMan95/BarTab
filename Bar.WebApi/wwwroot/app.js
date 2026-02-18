let currentTableData = null; // holds last loaded table detail

const apiBase = "";  // same origin
const BASE_TABLE_COUNT = 14; // fixed tables (1..14), bar tables are > 14

let currentTableId = null;
let menuItems = [];
let currentCategory = "All";
let menuSearchTerm = "";

let adminMenuItems = [];
let adminTempId = -1;

let adminEdits = new Map();     // key: id, value: edited object
let adminDeletedIds = new Set();// ids marked for delete

let adminSearchTerm = "";  

function onAdminSearchChange(value) {
    adminSearchTerm = (value || "").trim().toLowerCase();
    renderMenuAdminRows();
}

function resetTableView() {
    const title = document.getElementById("table-title");
    const subtitle = document.getElementById("table-subtitle");
    const status = document.getElementById("table-status");
    const content = document.getElementById("table-content");

    title.textContent = "Select a table";
    subtitle.textContent = "No table selected.";
    status.innerHTML = "";

    content.innerHTML = `
    <p style="font-size:.85rem;color:#9ca3af;">
        Choose a table from the list on the left to see its current order.
    </p>`;
}

async function fetchJson(url, options) {
    const res = await fetch(url, options);
    if (!res.ok) {
        const text = await res.text();
        throw new Error(text || res.statusText);
    }
    return res.status === 204 ? null : res.json();
}

// -------- Tables list & creation --------

async function loadTables() {
    const data = await fetchJson(apiBase + "/api/tables");
    const listEl = document.getElementById("tables-list");
    listEl.innerHTML = "";

    data.forEach(t => {
        const li = document.createElement("li");
        li.className = "table-item";
        if (t.id === currentTableId) li.classList.add("active");
        li.onclick = () => selectTable(t.id);

        const left = document.createElement("div");

        const isBarTable = t.id > BASE_TABLE_COUNT;
        let label;

        if (!isBarTable) {
            // Normal fixed tables 1..14
            label = "Table " + t.id;
            if (t.name && t.name !== label) {
                label += " – " + t.name;
            }
        } else {
            // Dynamic bar tables: preferably show the customer name
            const barIndex = t.id - BASE_TABLE_COUNT;

            if (t.name && t.name.trim() !== "") {
                // Just show the name, e.g. "Giorgos"
                label = t.name.trim();
            } else {
                // Fallback if no name yet
                label = "Bar " + barIndex;
            }
        }

        left.textContent = label;

        const right = document.createElement("div");
        right.style.display = "flex";
        right.style.flexDirection = "column";
        right.style.alignItems = "flex-end";

        const pill = document.createElement("div");
        pill.className = "pill " + (t.open ? "pill-open" : "pill-closed");
        pill.textContent = t.open ? "Open" : "Free";

        const total = document.createElement("div");
        total.className = "money";
        total.textContent = t.total.toFixed(2) + " €";

        right.appendChild(pill);
        right.appendChild(total);

        li.appendChild(left);
        li.appendChild(right);
        listEl.appendChild(li);
    });
}

async function saveTableNameForId(id, name) {
    await fetchJson(apiBase + `/api/tables/${id}/name`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name: name })
    });
}

async function createTable() {
    let name = prompt("Customer name for this bar table (optional):", "");
    if (name) {
        name = name.trim();
    }

    // 1) Create the bar table as before
    const newTable = await fetchJson(apiBase + "/api/tables", {
        method: "POST"
    });

    if (newTable && typeof newTable.id === "number") {
        currentTableId = newTable.id;

        // 2) If user provided a name, save it to the table
        if (name) {
            await saveTableNameForId(newTable.id, name);
        }
    }

    // 3) Refresh UI
    await loadTables();

    if (currentTableId !== null) {
        await loadTableDetails(currentTableId);
    }
}

// -------- Register --------

async function loadRegister() {
    try {
        const data = await fetchJson(apiBase + "/api/register");
        document.getElementById("reg-cash").textContent = data.cash.toFixed(2) + " €";
        document.getElementById("reg-card").textContent = data.card.toFixed(2) + " €";
        document.getElementById("reg-pending").textContent = data.pending.toFixed(2) + " €";
        document.getElementById("reg-tips").textContent = data.tips.toFixed(2) + " €";
    } catch (err) {
        console.error(err);
    }
}

// -------- Menu --------

async function loadMenu() {
    menuItems = await fetchJson(apiBase + "/api/menu");
    document.getElementById("menu-count").textContent =
        menuItems.length + " items";

    await loadMenuCategories();
    renderMenuGrid();
}

async function loadMenuCategories() {
    let categories = [];
    try {
        categories = await fetchJson(apiBase + "/api/menu/categories");
    } catch (err) {
        console.error("Error loading categories:", err);
    }

    const filtersEl = document.getElementById("menu-filters");
    if (!filtersEl) return;

    filtersEl.innerHTML = "";

    // Always add an "All" pill
    const allPill = document.createElement("button");
    allPill.className = "menu-filter-pill" + (currentCategory === "All" ? " active" : "");
    allPill.textContent = "All";
    allPill.onclick = () => {
        currentCategory = "All";
        updateFilterPills();
        renderMenuGrid();
    };
    filtersEl.appendChild(allPill);

    categories.forEach(cat => {
        const pill = document.createElement("button");
        pill.className = "menu-filter-pill" + (currentCategory === cat ? " active" : "");
        pill.textContent = cat;
        pill.onclick = () => {
            currentCategory = cat;
            updateFilterPills();
            renderMenuGrid();
        };
        filtersEl.appendChild(pill);
    });
}

function updateFilterPills() {
    const filtersEl = document.getElementById("menu-filters");
    if (!filtersEl) return;
    const pills = filtersEl.querySelectorAll(".menu-filter-pill");
    pills.forEach(pill => {
        const txt = pill.textContent;
        if (txt === currentCategory || (currentCategory === "All" && txt === "All")) {
            pill.classList.add("active");
        } else {
            pill.classList.remove("active");
        }
    });
}

function renderMenuGrid() {
    const grid = document.getElementById("menu-grid");
    if (!grid) return;

    grid.innerHTML = "";

    let items = menuItems;

    // Filter by category
    if (currentCategory !== "All") {
        items = items.filter(i => i.category === currentCategory);
    }

    // Filter by search term (name)
    if (menuSearchTerm) {
        items = items.filter(i =>
            (i.name || "").toLowerCase().includes(menuSearchTerm)
        );
    }

    items.forEach(item => {
        const card = document.createElement("div");
        card.className = "menu-item";

        const nameEl = document.createElement("div");
        nameEl.className = "menu-name";
        nameEl.textContent = item.name;

        const meta = document.createElement("div");
        meta.className = "menu-meta";

        const cat = document.createElement("span");
        cat.className = "badge";
        cat.textContent = item.category;

        const price = document.createElement("span");
        price.className = "money";
        price.textContent = item.price.toFixed(2) + " €";

        const stock = document.createElement("span");
        stock.className = "badge";
        stock.textContent = (item.stockQuantity == null) ? "∞" : ("Stock: " + item.stockQuantity);

        meta.appendChild(cat);
        meta.appendChild(price);
        meta.appendChild(stock);

        const footer = document.createElement("div");
        footer.className = "menu-footer";
        footer.style.display = "flex";
        footer.style.justifyContent = "flex-end";
        footer.style.alignItems = "center";

        const outOfStock = (item.stockQuantity === 0);

        const addBtn = document.createElement("button");
        addBtn.className = "btn btn-primary";
        addBtn.textContent = outOfStock ? "Out of stock" : "Add to table";
        addBtn.disabled = outOfStock;
        addBtn.onclick = () => addItemToCurrentTable(item);

        footer.appendChild(addBtn);

        card.appendChild(nameEl);
        card.appendChild(meta);
        card.appendChild(footer);

        grid.appendChild(card);
    });
}


// -------- Table details --------

async function selectTable(id) {
    currentTableId = id;
    await loadTableDetails(id);
    await loadTables(); // refresh highlighting & totals
}

async function saveCurrentTableName(newName) {
    if (currentTableId === null) return;

    newName = (newName || "").trim();

    await saveTableNameForId(currentTableId, newName);

    // Refresh sidebar + details to reflect new name / labels
    await loadTables();
    await loadTableDetails(currentTableId);
}

async function loadTableDetails(id) {
    const data = await fetchJson(apiBase + "/api/tables/" + id);
    currentTableData = data;

    const title = document.getElementById("table-title");
    const subtitle = document.getElementById("table-subtitle");
    const status = document.getElementById("table-status");
    const content = document.getElementById("table-content");

    const isBarTable = data.id > BASE_TABLE_COUNT;
    let titleLabel;

    if (!isBarTable) {
        titleLabel = "Table " + data.id;
        if (data.name && data.name !== titleLabel) {
            titleLabel += " – " + data.name;
        }
    } else {
        const barIndex = data.id - BASE_TABLE_COUNT;
        // Prefer customer name; fallback to Bar X
        if (data.name && data.name.trim() !== "") {
            titleLabel = data.name.trim();
        } else {
            titleLabel = "Bar " + barIndex;
        }
    }

    title.textContent = titleLabel;

    subtitle.textContent = data.open
        ? "Open bill – " + data.total.toFixed(2) + " €"
        : "Currently free";

    status.innerHTML = "";
    const pill = document.createElement("div");
    pill.className = "pill " + (data.open ? "pill-open" : "pill-closed");
    pill.textContent = data.open ? "Open" : "Free";
    status.appendChild(pill);

    // If table is free and has no items
    if (!data.open && (!data.items || data.items.length === 0)) {
        const wrapper = document.createElement("div");
        const msg = document.createElement("p");
        msg.style.fontSize = ".85rem";
        msg.style.color = "#9ca3af";
        msg.textContent = "No active order. Use the menu on the right to start one.";

        wrapper.appendChild(msg);

        content.innerHTML = "";
        content.appendChild(wrapper);
        return;
    }

    // ---------- Table with items ----------

    const wrapper = document.createElement("div");
    wrapper.style.display = "flex";
    wrapper.style.flexDirection = "column";
    wrapper.style.height = "100%";

    const table = document.createElement("table");

    const thead = document.createElement("thead");
    thead.innerHTML = "<tr>" +
        "<th style='width:28px;'></th>" + // checkbox column
        "<th>Item</th>" +
        "<th>Category</th>" +
        "<th style='text-align:right;'>Price</th>" +
        "</tr>";
    table.appendChild(thead);

    const tbody = document.createElement("tbody");
    data.items.forEach((i, index) => {
        const tr = document.createElement("tr");

        // Checkbox cell
        const tdSel = document.createElement("td");
        tdSel.style.textAlign = "center";
        const cb = document.createElement("input");
        cb.type = "checkbox";
        cb.className = "item-select";
        cb.dataset.index = index.toString();
        cb.addEventListener("change", updateSelectedSummary);
        tdSel.appendChild(cb);

        const tdName = document.createElement("td");
        const tdCat = document.createElement("td");
        const tdPrice = document.createElement("td");
        tdPrice.style.textAlign = "right";

        tdName.textContent = i.name;
        tdCat.textContent = i.category;
        tdPrice.textContent = i.price.toFixed(2) + " €";

        tr.appendChild(tdSel);
        tr.appendChild(tdName);
        tr.appendChild(tdCat);
        tr.appendChild(tdPrice);
        tbody.appendChild(tr);
    });
    table.appendChild(tbody);

    // Scrollable area for items
    const scrollArea = document.createElement("div");
    scrollArea.className = "table-scroll";
    scrollArea.appendChild(table);

    // ---------- Footer (fixed at bottom) ----------

    const footer = document.createElement("div");
    footer.className = "table-footer";
    footer.style.display = "flex";
    footer.style.justifyContent = "space-between";
    footer.style.alignItems = "flex-start";

    // Left side: total + selected summary
    const leftSide = document.createElement("div");
    leftSide.style.display = "flex";
    leftSide.style.flexDirection = "column";
    leftSide.style.gap = ".25rem";

    const totalEl = document.createElement("div");
    totalEl.innerHTML =
        "<span style='color:#9ca3af;font-size:.8rem;'>Total:</span> " +
        "<span class='money' style='font-size:.9rem;'>" + data.total.toFixed(2) + " €</span>";

    const selectedEl = document.createElement("div");
    selectedEl.id = "selected-summary";
    selectedEl.style.fontSize = ".75rem";
    selectedEl.style.color = "#9ca3af";
    selectedEl.textContent = "Selected: 0 items – 0.00 €";

    leftSide.appendChild(totalEl);
    leftSide.appendChild(selectedEl);

    // Right side: name + tip + discount + buttons + move
    const rightSide = document.createElement("div");
    rightSide.style.display = "flex";
    rightSide.style.flexDirection = "column";
    rightSide.style.alignItems = "flex-end";
    rightSide.style.gap = ".3rem";

    // Custom name for table / bar
    const nameContainer = document.createElement("div");
    nameContainer.style.display = "flex";
    nameContainer.style.alignItems = "center";
    nameContainer.style.gap = ".25rem";

    const nameLabel = document.createElement("span");
    nameLabel.style.fontSize = ".75rem";
    nameLabel.style.color = "#9ca3af";
    nameLabel.textContent = "Name:";

    const nameInput = document.createElement("input");
    nameInput.id = "table-name-input";
    nameInput.className = "input-sm";
    nameInput.style.width = "140px";
    nameInput.placeholder = "Customer name";
    nameInput.value = (data.name || "").trim();

    const nameSave = document.createElement("button");
    nameSave.className = "btn btn-ghost btn-sm";
    nameSave.textContent = "Save";
    nameSave.onclick = () => {
        const newName = nameInput.value.trim();
        saveCurrentTableName(newName);
    };

    nameContainer.appendChild(nameLabel);
    nameContainer.appendChild(nameInput);
    nameContainer.appendChild(nameSave);

    rightSide.appendChild(nameContainer);

    // Tip input
    const tipContainer = document.createElement("div");
    tipContainer.style.display = "flex";
    tipContainer.style.alignItems = "center";
    tipContainer.style.gap = ".25rem";

    const tipLabel = document.createElement("span");
    tipLabel.style.fontSize = ".75rem";
    tipLabel.style.color = "#9ca3af";
    tipLabel.textContent = "Tip:";

    const tipInput = document.createElement("input");
    tipInput.type = "number";
    tipInput.min = "0";
    tipInput.step = "0.5";
    tipInput.value = "0";
    tipInput.className = "input-sm";
    tipInput.style.width = "60px";
    tipInput.id = "tip-input";

    tipContainer.appendChild(tipLabel);
    tipContainer.appendChild(tipInput);

    // Discount input (percentage)
    const discountContainer = document.createElement("div");
    discountContainer.style.display = "flex";
    discountContainer.style.alignItems = "center";
    discountContainer.style.gap = ".25rem";

    const discountLabel = document.createElement("span");
    discountLabel.style.fontSize = ".75rem";
    discountLabel.style.color = "#9ca3af";
    discountLabel.textContent = "Discount %:";

    const discountInput = document.createElement("input");
    discountInput.type = "number";
    discountInput.min = "0";
    discountInput.max = "100";
    discountInput.step = "1";
    discountInput.value = "0";
    discountInput.className = "input-sm";
    discountInput.style.width = "60px";
    discountInput.id = "discount-input";

    discountContainer.appendChild(discountLabel);
    discountContainer.appendChild(discountInput);

    // Buttons full pay
    const buttons = document.createElement("div");
    buttons.style.display = "flex";
    buttons.style.gap = ".4rem";

    const cardFee = 1.1;

    const btnCash = document.createElement("button");
    btnCash.className = "btn btn-primary";
    btnCash.onclick = () => closeCurrentTable("cash");

    const btnCard = document.createElement("button");
    btnCard.className = "btn btn";
    btnCard.onclick = () => closeCurrentTable("cash");

    // function to recalc labels when discount changes
    function updatePaymentLabels() {
        const discountPercent = parseFloat(discountInput.value) || 0;
        const clampedPercent = Math.min(Math.max(discountPercent, 0), 100);

        const discountAmount = data.total * (clampedPercent / 100);
        const totalAfterDiscount = Math.max(0, data.total - discountAmount);

        btnCash.textContent = `Pay all cash: ${totalAfterDiscount.toFixed(2)}€`;
        btnCard.textContent = `Pay all card: ${(totalAfterDiscount * cardFee).toFixed(2)}€`;
    }

    // update when the user types in the discount
    discountInput.addEventListener("input", updatePaymentLabels);

    // initial label setup
    updatePaymentLabels();

    buttons.appendChild(btnCash);
    buttons.appendChild(btnCard);


    // Buttons partial pay
    const partialButtons = document.createElement("div");
    partialButtons.style.display = "flex";
    partialButtons.style.gap = ".4rem";

    const btnSelCash = document.createElement("button");
    btnSelCash.className = "btn btn-sm";
    btnSelCash.textContent = "Pay selected (cash)";
    btnSelCash.onclick = () => paySelectedItems("cash");

    const btnSelCard = document.createElement("button");
    btnSelCard.className = "btn btn-sm";
    btnSelCard.textContent = "Pay selected (card)";
    btnSelCard.onclick = () => paySelectedItems("card");

    const btnRemove = document.createElement("button");
    btnRemove.className = "btn btn-ghost btn-sm";
    btnRemove.textContent = "Remove selected";
    btnRemove.onclick = () => removeSelectedItems();

    partialButtons.appendChild(btnSelCash);
    partialButtons.appendChild(btnSelCard);
    partialButtons.appendChild(btnRemove);

    // Move to another table
    const moveContainer = document.createElement("div");
    moveContainer.style.display = "flex";
    moveContainer.style.alignItems = "center";
    moveContainer.style.gap = ".25rem";
    moveContainer.style.marginTop = ".3rem";

    const moveInput = document.createElement("input");
    moveInput.id = "move-table-id";
    moveInput.className = "input-sm";
    moveInput.placeholder = "Move to table #";
    moveInput.type = "number";
    moveInput.min = "1";

    const moveBtn = document.createElement("button");
    moveBtn.className = "btn btn-ghost btn-sm";
    moveBtn.textContent = "Move";
    moveBtn.onclick = () => moveTableToTable();

    moveContainer.appendChild(moveInput);
    moveContainer.appendChild(moveBtn);

    rightSide.appendChild(tipContainer);
    rightSide.appendChild(discountContainer);
    rightSide.appendChild(buttons);
    rightSide.appendChild(partialButtons);
    rightSide.appendChild(moveContainer);

    footer.appendChild(leftSide);
    footer.appendChild(rightSide);

    // Assemble: scroll area + fixed footer
    wrapper.appendChild(scrollArea);
    wrapper.appendChild(footer);

    content.innerHTML = "";
    content.appendChild(wrapper);

    // initial selected summary
    updateSelectedSummary();
}

function updateSelectedSummary() {
    const summaryEl = document.getElementById("selected-summary");
    if (!summaryEl || !currentTableData || !currentTableData.items) return;

    const checkboxes = document.querySelectorAll(".item-select:checked");
    let count = 0;
    let sum = 0;

    checkboxes.forEach(cb => {
        const idx = parseInt(cb.dataset.index, 10);
        if (!isNaN(idx) && currentTableData.items[idx]) {
            count++;
            sum += currentTableData.items[idx].price;
        }
    });

    summaryEl.textContent =
        `Selected: ${count} item${count === 1 ? "" : "s"} – ${sum.toFixed(2)} €`;
}

async function paySelectedItems(method) {
    if (currentTableId === null) {
        alert("Select a table first.");
        return;
    }
    if (!currentTableData || !currentTableData.items) {
        alert("No items to pay.");
        return;
    }

    const checkboxes = document.querySelectorAll(".item-select:checked");
    const indices = [];
    checkboxes.forEach(cb => {
        const idx = parseInt(cb.dataset.index, 10);
        if (!isNaN(idx)) indices.push(idx);
    });

    if (indices.length === 0) {
        alert("Select at least one item.");
        return;
    }

    const tipInput = document.getElementById("tip-input");
    const tip = tipInput ? parseFloat(tipInput.value) || 0 : 0;

    const discountInput = document.getElementById("discount-input");
    const discountPercent = discountInput ? parseFloat(discountInput.value) || 0 : 0;

    await fetchJson(apiBase + "/api/tables/" + currentTableId + "/pay-items", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            paymentMethod: method,
            tip: tip,
            itemIndexes: indices,
            discountPercent: discountPercent
        })
    });

    await loadTables();
    await loadRegister();
    await loadTableDetails(currentTableId);
}

async function removeSelectedItems() {
    if (currentTableId === null) {
        alert("Select a table first.");
        return;
    }
    if (!currentTableData || !currentTableData.items) {
        alert("No items to remove.");
        return;
    }

    const checkboxes = document.querySelectorAll(".item-select:checked");
    const indices = [];
    checkboxes.forEach(cb => {
        const idx = parseInt(cb.dataset.index, 10);
        if (!isNaN(idx)) indices.push(idx);
    });

    if (indices.length === 0) {
        alert("Select at least one item to remove.");
        return;
    }

    await fetchJson(apiBase + "/api/tables/" + currentTableId + "/remove-items", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            itemIndexes: indices
        })
    });

    // IMPORTANT: refresh menu so stock numbers update after restock
    await Promise.all([
        loadTables(),
        loadRegister(),
        loadTableDetails(currentTableId),
        loadMenu()
    ]);
}


// -------- Actions: add item, pay, move --------

async function addItemToCurrentTable(menuItem) {
    if (currentTableId === null) {
        alert("Select a table first.");
        return;
    }

    if (!menuItem || menuItem.index == null) {
        alert("Invalid menu item.");
        return;
    }

    // Frontend safety check (backend must still enforce stock)
    // stockQuantity: null/undefined => unlimited
    if (menuItem.stockQuantity === 0) {
        alert("Out of stock.");
        return;
    }

    const body = {
        menuItemId: menuItem.index
    };

    try {
        await fetchJson(apiBase + "/api/tables/" + currentTableId + "/items", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        // Refresh UI to reflect:
        // - new table total/items
        // - register pending
        // - menu stock decremented
        await Promise.all([
            loadTableDetails(currentTableId),
            loadTables(),
            loadRegister(),
            loadMenu()
        ]);
    } catch (err) {
        // fetchJson throws Error(text). The backend should return a useful message.
        const msg = (err && err.message) ? err.message : "Could not add item.";

        // Optional: if backend says out-of-stock, refresh menu anyway to show 0
        if (msg.toLowerCase().includes("out of stock")) {
            await loadMenu();
        }

        alert(msg);
    }
}



async function closeCurrentTable(method) {
    if (currentTableId === null) return;

    const tipInput = document.getElementById("tip-input");
    const tip = tipInput ? parseFloat(tipInput.value) || 0 : 0;

    const discountInput = document.getElementById("discount-input");
    const discountPercent = discountInput ? parseFloat(discountInput.value) || 0 : 0;

    const closedId = currentTableId;

    // If user typed a new name, save it first
    const nameInput = document.getElementById("table-name-input");
    if (nameInput) {
        const newName = nameInput.value.trim();
        const oldName = (currentTableData && currentTableData.name) ? currentTableData.name.trim() : "";
        if (newName !== oldName) {
            await saveTableNameForId(closedId, newName);
        }
    }

    await fetchJson(apiBase + "/api/tables/" + closedId + "/close", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            paymentMethod: method,
            tip: tip,
            discountPercent: discountPercent
        })
    });

    await loadTables();
    await loadRegister();

    if (closedId > BASE_TABLE_COUNT) {
        currentTableId = null;
        resetTableView();
    } else {
        await loadTableDetails(closedId);
    }
}

async function moveTableToTable() {
    if (currentTableId === null) return;

    const input = document.getElementById("move-table-id");
    if (!input) return;

    const targetId = parseInt(input.value, 10);
    if (isNaN(targetId) || targetId <= 0) {
        alert("Enter a valid target table number.");
        return;
    }
    if (targetId === currentTableId) {
        alert("Cannot move to the same table.");
        return;
    }

    await fetchJson(apiBase + "/api/tables/" + currentTableId + "/move-to-table", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ targetTableId: targetId })
    });

    input.value = "";

    await loadTables();
    await loadRegister();
    await selectTable(targetId); // show the new table where the bill is
}

// -------- Global refresh --------

async function refreshAll() {
    await Promise.all([
        loadTables(),
        loadRegister(),
        loadMenu()
    ]);
}

// init
window.addEventListener("load", () => {
    refreshAll().catch(err => console.error(err));
});

function openMenuAdmin() {

    adminSearchTerm = "";
    const input = document.getElementById("admin-search-input");
    if (input) input.value = "";

    const modal = document.getElementById("menu-admin-modal");
    modal.style.display = "flex";
    loadMenuAdmin().catch(err => {
        console.error(err);
        alert("Could not load menu admin.");
    });
}

function closeMenuAdmin() {
    const modal = document.getElementById("menu-admin-modal");
    modal.style.display = "none";
}

async function loadMenuAdmin() {
    setAdminStatus("Loading...");
    adminMenuItems = await fetchJson(apiBase + "/api/menu/all", { cache: "no-store" });

    adminEdits.clear();
    adminDeletedIds.clear();

    renderMenuAdminRows();
    setAdminStatus(`Loaded ${adminMenuItems.length} items`);
}

function setAdminStatus(text) {
    const el = document.getElementById("admin-status");
    if (el) el.textContent = text || "";
}

function renderMenuAdminRows() {
    const host = document.getElementById("admin-rows");
    if (!host) return;

    host.innerHTML = "";

    let items = adminMenuItems;

    // Search filter (name)
    if (adminSearchTerm) {
        items = items.filter(item => {
            // use edited name if user changed it
            const current = adminEdits.get(item.index) || item;
            const name = (current.name || "").toLowerCase();
            return name.includes(adminSearchTerm);
        });
    }

    items.forEach(item => {
        const row = document.createElement("div");
        row.className = "admin-row";
        row.dataset.id = item.index;

        // If marked deleted: visually dim it
        if (adminDeletedIds.has(item.index)) {
            row.style.opacity = "0.45";
        }

        // Helper: get "current" value = edits override original
        const current = adminEdits.get(item.index) || { ...item };

        // Inputs
        const nameInput = document.createElement("input");
        nameInput.className = "input-sm";
        nameInput.type = "text";
        nameInput.value = current.name || "";
        nameInput.placeholder = "Name";

        const catInput = document.createElement("input");
        catInput.className = "input-sm";
        catInput.type = "text";
        catInput.value = current.category || "";
        catInput.placeholder = "Category";

        const priceInput = document.createElement("input");
        priceInput.className = "input-sm";
        priceInput.type = "number";
        priceInput.step = "0.1";
        priceInput.min = "0";
        priceInput.value = (typeof current.price === "number") ? current.price : 0;

        const stockInput = document.createElement("input");
        stockInput.className = "input-sm";
        stockInput.type = "number";
        stockInput.step = "1";
        stockInput.min = "0";
        stockInput.value = (current.stockQuantity === null || current.stockQuantity === undefined)
            ? ""
            : current.stockQuantity;

        const activeInput = document.createElement("input");
        activeInput.type = "checkbox";
        activeInput.checked = !!current.active;
        activeInput.style.transform = "scale(1.1)";

        // Disable editing if deleted (optional)
        const disabled = adminDeletedIds.has(item.index);
        nameInput.disabled = disabled;
        catInput.disabled = disabled;
        priceInput.disabled = disabled;
        stockInput.disabled = disabled;
        activeInput.disabled = disabled;

        // Whenever inputs change, store edits
        function commitEdit() {
            if (adminDeletedIds.has(item.index)) return;

            const edited = {
                index: item.index,
                name: nameInput.value,
                category: catInput.value,
                price: priceInput.value,
                stockQuantity: stockInput.value,
                active: activeInput.checked
            };
            adminEdits.set(item.index, edited);
            setAdminStatus("Unsaved changes");
        }

        nameInput.addEventListener("input", commitEdit);
        catInput.addEventListener("input", commitEdit);
        priceInput.addEventListener("input", commitEdit);
        stockInput.addEventListener("input", commitEdit);
        activeInput.addEventListener("change", commitEdit);

        // Actions: only Delete/Undo delete
        const actions = document.createElement("div");
        actions.className = "admin-actions";

        const delBtn = document.createElement("button");
        delBtn.className = "btn btn-ghost btn-sm";

        if (adminDeletedIds.has(item.index)) {
            delBtn.textContent = "Undo";
            delBtn.onclick = () => {
                adminDeletedIds.delete(item.index);
                renderMenuAdminRows();
                setAdminStatus("Unsaved changes");
            };
        } else {
            delBtn.textContent = "Delete";
            delBtn.onclick = () => {
                // New unsaved row: remove immediately
                if (item.index < 0) {
                    adminMenuItems = adminMenuItems.filter(x => x.index !== item.index);
                    adminEdits.delete(item.index);
                    renderMenuAdminRows();
                    setAdminStatus("Unsaved changes");
                    return;
                }

                // Existing row: mark for delete
                adminDeletedIds.add(item.index);
                adminEdits.delete(item.index); // no need to keep edits if deleting
                renderMenuAdminRows();
                setAdminStatus("Unsaved changes");
            };
        }

        actions.appendChild(delBtn);

        // Layout
        row.appendChild(nameInput);
        row.appendChild(catInput);

        const priceWrap = document.createElement("div");
        priceWrap.style.textAlign = "right";
        priceWrap.appendChild(priceInput);
        row.appendChild(priceWrap);

        const stockWrap = document.createElement("div");
        stockWrap.style.textAlign = "right";
        stockWrap.appendChild(stockInput);
        row.appendChild(stockWrap);

        const activeWrap = document.createElement("div");
        activeWrap.style.textAlign = "center";
        activeWrap.appendChild(activeInput);
        row.appendChild(activeWrap);

        row.appendChild(actions);

        host.appendChild(row);
    });
}

function adminAddNewRow() {
    const newItem = {
        index: adminTempId--,
        name: "",
        category: "",
        price: 0,
        active: true,
        stockQuantity: null
    };

    adminMenuItems.unshift(newItem);

    // create an edit entry so Save All knows about it
    adminEdits.set(newItem.index, {
        index: newItem.index,
        name: "",
        category: "",
        price: "0",
        stockQuantity: "",
        active: true
    });

    renderMenuAdminRows();
    setAdminStatus("Unsaved changes");
}

async function adminDeleteRow(id, name) {
    if (id < 0) {
        // Unsaved new row, just remove locally
        adminMenuItems = adminMenuItems.filter(x => x.index !== id);
        renderMenuAdminRows();
        return;
    }

    if (!confirm(`Delete "${name}"? (This will set Active=false)`)) return;

    try {
        setAdminStatus("Deleting...");
        await fetchJson(apiBase + `/api/menu/${id}`, { method: "DELETE" });

        await loadMenuAdmin();
        await loadMenu();
        setAdminStatus("Deleted ✅");
    } catch (err) {
        console.error(err);
        alert(err.message || "Delete failed.");
        setAdminStatus("Delete failed");
    }
}

async function adminSaveAll() {
    // If nothing changed:
    if (adminEdits.size === 0 && adminDeletedIds.size === 0) {
        alert("No changes to save.");
        return;
    }

    setAdminStatus("Saving...");

    // Build lookups for originals
    const originalById = new Map(adminMenuItems.map(x => [x.index, x]));

    // Helper to normalize inputs into API payload
    function buildPayload(edited) {
        const name = (edited.name || "").trim();
        const category = (edited.category || "").trim();

        const price = parseFloat((edited.price || "0").toString().replace(",", "."));
        if (!name || !category || isNaN(price) || price <= 0) {
            throw new Error("Each item must have name, category, and valid price > 0.");
        }

        let stockQuantity = null;
        const stockText = (edited.stockQuantity ?? "").toString().trim();
        if (stockText !== "") {
            const s = parseInt(stockText, 10);
            if (isNaN(s) || s < 0) throw new Error("Stock must be empty (∞) or a number >= 0.");
            stockQuantity = s;
        }

        return {
            name,
            category,
            price,
            active: !!edited.active,
            stockQuantity
        };
    }

    // Determine which existing rows are truly changed
    const creates = [];
    const updates = [];
    const deletes = Array.from(adminDeletedIds);

    try {
        for (const [id, edited] of adminEdits.entries()) {
            // Skip deleted ids
            if (adminDeletedIds.has(id)) continue;

            const payload = buildPayload(edited);

            if (id < 0) {
                creates.push(payload);
            } else {
                const original = originalById.get(id);
                if (!original) continue;

                // Compare to avoid unnecessary PUTs
                const same =
                    (payload.name === original.name) &&
                    (payload.category === original.category) &&
                    (Number(payload.price) === Number(original.price)) &&
                    (payload.active === !!original.active) &&
                    ((payload.stockQuantity ?? null) === (original.stockQuantity ?? null));

                if (!same) {
                    updates.push({ id, payload });
                }
            }
        }

        // Execute requests (sequential for simplicity & clearer errors)
        // (You can parallelize later if you want)
        for (const p of creates) {
            await fetchJson(apiBase + "/api/menu", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(p)
            });
        }

        for (const u of updates) {
            await fetchJson(apiBase + `/api/menu/${u.id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(u.payload)
            });
        }

        for (const id of deletes) {
            await fetchJson(apiBase + `/api/menu/${id}`, {
                method: "DELETE"
            });
        }

        // Reload admin + refresh PDA menu
        await loadMenuAdmin();
        await loadMenu();

        setAdminStatus("Saved ✅");
        alert(`Saved!\nCreated: ${creates.length}\nUpdated: ${updates.length}\nDeleted: ${deletes.length}`);
    } catch (err) {
        console.error(err);
        setAdminStatus("Save failed");
        alert(err.message || "Save failed.");
    }
}

function onMenuSearchChange(value) {
    menuSearchTerm = (value || "").trim().toLowerCase();
    renderMenuGrid();
}

async function closeRegister() {
    try {
        // Get register first to check pending
        const reg = await fetchJson(apiBase + "/api/register");

        if (reg.pending !== 0) {
            alert("Cannot close register. Pending must be 0.00 €");
            return;
        }

        if (!confirm("Close register and reset Cash/Card/Tips to 0?")) {
            return;
        }

        await fetchJson(apiBase + "/api/register/close", {
            method: "POST"
        });

        await loadRegister();
        alert("Register closed and reset.");
    } catch (err) {
        alert(err.message || "Could not close register.");
    }
}


