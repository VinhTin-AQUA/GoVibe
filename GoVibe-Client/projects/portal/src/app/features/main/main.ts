import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainHeader } from './components/main-header/main-header';

@Component({
  selector: 'app-main',
  imports: [RouterOutlet, MainHeader],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {

}
