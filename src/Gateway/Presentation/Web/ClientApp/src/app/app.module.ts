import { BrowserModule, BrowserTransferStateModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { ServiceWorkerModule } from '@angular/service-worker';

// import { PrebootModule } from 'preboot';

import { environment } from '../environments/environment';

import { routes } from './app.routes';
// Components
import { FooterComponent, HeaderComponent, PrivacyComponent } from '@app/components';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SharedModule } from './shared';
import { BlogModule } from './blog/blog.module';
import { StorageManagementModule } from './storage-management/storage-management.module';

@NgModule({
  declarations: [
    // Components
    AppComponent,
    HomeComponent,
    FooterComponent,
    HeaderComponent,
    PrivacyComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    // PrebootModule.withConfig({ appRoot: 'appc-root' }),
    BrowserAnimationsModule,
    BrowserTransferStateModule,
    RouterModule.forRoot(routes, { initialNavigation: 'enabled' }),
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    SharedModule,
    BlogModule,
    StorageManagementModule
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
