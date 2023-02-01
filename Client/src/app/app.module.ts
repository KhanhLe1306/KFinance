import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LineChartComponent } from './Components/line-chart/line-chart.component';
import { CalculateFormComponent } from './Components/calculate-form/calculate-form.component';

@NgModule({
  declarations: [
    AppComponent,
    LineChartComponent,
    CalculateFormComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule, 
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
