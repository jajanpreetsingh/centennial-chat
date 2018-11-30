import { Component } from '@angular/core';

@Component({
  selector: 'app-icons',
  templateUrl: './icons.component.html',
  styleUrls: ['./icons.component.scss']
})
export class IconsComponent {

  setIconName(object) {
    let name: string = object.src;
    console.log(object);
    console.log(object.src);
    console.log(name);
  }
}
