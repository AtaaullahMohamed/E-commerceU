import { Directive, effect, inject, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../../core/services/account.service';

@Directive({
  selector: '[appIsAdmin]', //*appIsAdmin
  standalone: true
})
export class IsAdminDirective {
  private accountService = inject(AccountService);
  private viewContainerref = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef)


  constructor() {
    effect(() => {
      if (this.accountService.isAdmin()) {
        this.viewContainerref.createEmbeddedView(this.templateRef);

      }
      else {
        this.viewContainerref.clear();

      }
    })
  }
}




