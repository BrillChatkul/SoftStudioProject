let calendarEl = doccument.getElementById('calendar');

let calendar = new FullCalendar.Calendar(calendarEl, {
    initialView: 'dayGridMonth',
    headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay'
    },
    events: [
        {
            title: "today",
            start: "2022-04-25",
        },
    ],
});

calendar.rander();