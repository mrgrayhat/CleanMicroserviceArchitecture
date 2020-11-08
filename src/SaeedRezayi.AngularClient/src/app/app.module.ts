import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { AuthenticationModule } from "./authentication/authentication.module";
import { BlogModule } from './blog/blog.module';
import { CoreModule } from "./core/core.module";
import { DashboardModule } from "./dashboard/dashboard.module";
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { SharedModule } from "./shared/shared.module";
import { WelcomeComponent } from "./welcome/welcome.component";
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';

@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    CoreModule,
    SharedModule.forRoot(),
    AuthenticationModule,
    BlogModule,
    DashboardModule,
    AppRoutingModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }