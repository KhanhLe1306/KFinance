import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FMPServiceService } from 'src/app/Services/fmp-service.service';

@Component({
  selector: 'app-calculate-form',
  templateUrl: './calculate-form.component.html',
  styleUrls: ['./calculate-form.component.css'],
})
export class CalculateFormComponent implements OnInit {
  calculateForm = new FormGroup({
    tickerSymbol: new FormControl('', Validators.required),
    start: new FormControl('', Validators.required),
    end: new FormControl('', Validators.required),
    amount: new FormControl(0, Validators.required),
    frequency: new FormControl(0, Validators.required),
  });
  totalInvested: number;
  currentInvestment: number;
  percentageChange: number;
  data: any;

  constructor(private FMPService: FMPServiceService) {}

  ngOnInit(): void {}

  onSubmit() {
    console.log(this.calculateForm.value);
    this.FMPService.getROI(this.calculateForm.value).subscribe({
      next: (data) => {
        console.log(data);
        this.data = data;
      },
      error: (err) => console.log(err),
    }); 
  }
}
