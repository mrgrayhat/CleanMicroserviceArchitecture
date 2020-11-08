import { CommonModule } from "@angular/common";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { APP_INITIALIZER, NgModule, Optional, SkipSelf } from "@angular/core";
import { RouterModule } from "@angular/router";

import { HeaderComponent } from "./components/header/header.component";
import { ApiConfigService } from "./services/api/api-config.service";
import { APP_CONFIG, AppConfig } from "./services/api/app.config";
import { AuthInterceptor } from "./services/auth/auth.interceptor";
import { XsrfInterceptor } from "./services/auth/xsrf.interceptor";;
import { FooterComponent } from './components/footer/footer.component'

// import { SeoService } from './services/seo/SeoService';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule
  ],
  exports: [
    // components that are used in app.component.ts will be listed here.
    HeaderComponent,
    FooterComponent

  ],
  declarations: [
    // components that are used in app.component.ts will be listed here.
    HeaderComponent,
    FooterComponent
  ],

  providers: [
    /* ``No`` global singleton services of the whole app should be listed here anymore!
       Since they'll be already provided in AppModule using the `tree-shakable providers` of Angular 6.x+ (providedIn: 'root').
       This new feature allows cleaning up the providers section from the CoreModule.
       But if you want to provide something with an InjectionToken other that its class, you still have to use this section.
    */
    // SeoService,
    {
      provide: APP_CONFIG,
      useValue: AppConfig
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: XsrfInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: (config: ApiConfigService) => () => config.loadApiConfig(),
      deps: [ApiConfigService],
      multi: true
    }
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error("CoreModule should be imported ONLY in AppModule.");
    }
  }
}
