import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { WelcomeComponent } from "./welcome/welcome.component";

const routes: Routes = [
  {
    path: "welcome", component: WelcomeComponent,
    data: {
      title: "وبلاگ سعید رضایی",
      metaTags: {
        description: "Saeed Rezayi Blog",
        keywords: "blog,saeedrezayi,weblog"
      }
    }
  },
  { path: "", redirectTo: "welcome", pathMatch: "full" },
  { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) },
  { path: "**", component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }