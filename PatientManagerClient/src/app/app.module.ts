import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from '../material-module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ModalpopupComponent } from './modalpopup/modalpopup.component';
import { UploadComponent } from './upload/upload.component';

import { DefaultUrlSerializer, UrlSerializer, UrlTree } from '@angular/router';

export class LowerCaseUrlSerializer extends DefaultUrlSerializer {
  override parse(url: string): UrlTree {
      // Added to support case insensitive urls
      return super.parse(url.toLowerCase()); 
  }
}

@NgModule({
  declarations: [
    AppComponent,
    ModalpopupComponent,
    UploadComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    {
    provide: UrlSerializer,
    useClass: LowerCaseUrlSerializer
  }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

