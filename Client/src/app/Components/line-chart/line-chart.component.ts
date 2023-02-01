import { Component, OnInit } from '@angular/core';
import { Chart, registerables } from 'node_modules/chart.js';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css'],
})
export class LineChartComponent implements OnInit {
  myChart: any;
  label: any = ['2010', '2011', '2012', '2013', '2014'];
  constructor() {
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    this.myChart = new Chart('myChart', {
      type: 'line',
      data: {
        labels: this.label,
        datasets: [
          {
            label: '# of Votes',
            data: [12, 19, 3, 5, 2],
            borderWidth: 1,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1,
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });
  }
}
