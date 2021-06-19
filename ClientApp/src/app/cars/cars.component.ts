import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Car } from './car.model';
import { CarsService } from './cars.service';

@Component({
  selector: 'app-cars',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.css']
})
export class CarsComponent  {

  public cars: Car[];

  constructor(private carsService: CarsService) {

  }

  getCars() {
    this.carsService.getCars().subscribe(c => this.cars = c);
  }

  ngOnInit() {
    this.getCars();
  }

}
