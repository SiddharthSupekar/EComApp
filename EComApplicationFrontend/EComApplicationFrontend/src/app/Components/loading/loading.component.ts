import { Component } from '@angular/core';
import { LoaderService } from '../../Services/Loader/loader.service';
import { delay } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.css'
})
export class LoadingComponent {
  loading:boolean = false;
  constructor(private _loading : LoaderService){}

  ngOnInit():void{
    this.listenToLoading()
  }

  listenToLoading():void{

      this._loading.loadingSub.pipe(delay(10)).subscribe((loading)=>{
      this.loading = loading;
    })
  }

}
