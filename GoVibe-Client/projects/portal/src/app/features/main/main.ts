import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainHeader } from './components/main-header/main-header';
import { MainFooter } from './components/main-footer/main-footer';

@Component({
  selector: 'app-main',
  imports: [RouterOutlet, MainHeader, MainFooter],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {

}
