let eventsArr = loadData();
let calendar = initCalendar();

function loadData() {
    let eventsArr = [];

    let todoTable = document.getElementById("todoTable");
    let trElem = todoTable.getElementsByTagName("tr");
    console.log(trElem);
    for (let tr of trElem) {
        console.log(tr);
        let tdElems = tr.getElementsByTagName("td");
        let eventObj = {
            id: tdElems[0].innerText,
            title: tdElems[1].innerText,
            start: tdElems[2].innerText,
        }

        eventsArr.push(eventObj);
    }

    return eventsArr;
}

function initCalendar() {
    var calendarEl = document.getElementById('calendar');

    let calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: eventsArr,
    });

calendar.render();