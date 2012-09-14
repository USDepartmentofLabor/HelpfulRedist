        function drawVisualization() {
            // Create and populate the data table.
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'x');
            data.addColumn('number', 'Volume of Feedback');
            data.addColumn('number', 'Volume of Negative Feedback');
            data.addColumn('number', 'Volume of Positive Feedback');
            data.addRow(["Jan 10", 10000, 1, 0.5]);
            data.addRow(["Feb 10", 20000, 0.5, 1]);
            data.addRow(["Mar 10", 40000, 1, 0.5]);
            data.addRow(["Apr 10", 80000, 0.5, 1]);
            data.addRow(["May 10", 70000, 1, 0.5]);
            data.addRow(["Jun 10", 70000, 0.5, 1]);
            data.addRow(["Jul 10", 80000, 1, 0.5]);
            data.addRow(["Aug 10", 40000, 0.5, 1]);
            data.addRow(["Sep 10", 20000, 1, 0.5]);
            data.addRow(["Oct 10", 30005, 0.5, 1]);
            data.addRow(["Nov 10", 30000, 1, 0.5]);
            data.addRow(["Dec 10", 40005, 0.5, 1]);
            data.addRow(["Jan 11", 100000, 1, 0.5]);
            data.addRow(["Feb 11", 200000, 0.5, 1]);
            data.addRow(["Mar 11", 400000, 1, 0.5]);
            data.addRow(["Apr 11", 800000, 0.5, 1]);
            data.addRow(["May 11", 700000, 1, 0.5]);
            data.addRow(["Jun 11", 700000, 0.5, 1]);
            data.addRow(["Jul 11", 800000, 1, 0.5]);
            data.addRow(["Aug 11", 400000, 0.5, 1]);
            data.addRow(["Sep 11", 200000, 1, 0.5]);
            data.addRow(["Oct 11", 300005, 0.5, 1]);
            data.addRow(["Nov 11", 300000, 1, 0.5]);
            data.addRow(["Dec 11", 400005, 0.5, 1]);
            data.addRow(["Jan 12", 300000, 1, 0.5]);
            data.addRow(["Feb 12", 400000, 0.5, 1]);
            data.addRow(["Mar 12", 500000, 1, 0.5]);
            data.addRow(["Apr 12", 600000, 0.5, 1]);
            data.addRow(["May 12", 700000, 1, 0.5]);
            data.addRow(["Jun 12", 800000, 0.5, 1]);
            data.addRow(["Jul 12", 800000, 1, 0.5]);
            data.addRow(["Aug 12", 1100000, 0.5, 1]);
            data.addRow(["Sep 12", 220000, 1, 0.5]);
            data.addRow(["Oct 12", 330005, 0.5, 1]);
            data.addRow(["Nov 12", 340000, 1, 0.5]);
            data.addRow(["Dec 12", 440005, 0.5, 1]);

            // Create and draw the visualization.
            new google.visualization.LineChart(document.getElementById('visualization')).
            draw(data, { curveType: "function",
                width: 1000, height: 400,
                vAxis: { maxValue: 100000}
            });
        }
        google.setOnLoadCallback(drawVisualization);
    