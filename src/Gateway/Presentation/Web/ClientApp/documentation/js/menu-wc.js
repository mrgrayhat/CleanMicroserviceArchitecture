'use strict';


customElements.define('compodoc-menu', class extends HTMLElement {
    constructor() {
        super();
        this.isNormalMode = this.getAttribute('mode') === 'normal';
    }

    connectedCallback() {
        this.render(this.isNormalMode);
    }

    render(isNormalMode) {
        let tp = lithtml.html(`
        <nav>
            <ul class="list">
                <li class="title">
                    <a href="index.html" data-type="index-link">aspnetnetcore documentation</a>
                </li>

                <li class="divider"></li>
                ${ isNormalMode ? `<div id="book-search-input" role="search"><input type="text" placeholder="Type to search"></div>` : '' }
                <li class="chapter">
                    <a data-type="chapter-link" href="index.html"><span class="icon ion-ios-home"></span>Getting started</a>
                    <ul class="links">
                        <li class="link">
                            <a href="index.html" data-type="chapter-link">
                                <span class="icon ion-ios-keypad"></span>Overview
                            </a>
                        </li>
                                <li class="link">
                                    <a href="dependencies.html" data-type="chapter-link">
                                        <span class="icon ion-ios-list"></span>Dependencies
                                    </a>
                                </li>
                    </ul>
                </li>
                    <li class="chapter modules">
                        <a data-type="chapter-link" href="modules.html">
                            <div class="menu-toggler linked" data-toggle="collapse" ${ isNormalMode ?
                                'data-target="#modules-links"' : 'data-target="#xs-modules-links"' }>
                                <span class="icon ion-ios-archive"></span>
                                <span class="link-name">Modules</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                        </a>
                        <ul class="links collapse " ${ isNormalMode ? 'id="modules-links"' : 'id="xs-modules-links"' }>
                            <li class="link">
                                <a href="modules/AppModule.html" data-type="entity-link">AppModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#components-links-module-AppModule-f8b9cd04cb162b4fe3ae547f33ca92c0"' : 'data-target="#xs-components-links-module-AppModule-f8b9cd04cb162b4fe3ae547f33ca92c0"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppModule-f8b9cd04cb162b4fe3ae547f33ca92c0"' :
                                            'id="xs-components-links-module-AppModule-f8b9cd04cb162b4fe3ae547f33ca92c0"' }>
                                            <li class="link">
                                                <a href="components/AppComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AppComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FooterComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FooterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HeaderComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">HeaderComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HomeComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">HomeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PrivacyComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">PrivacyComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ExamplesModule.html" data-type="entity-link">ExamplesModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#components-links-module-ExamplesModule-22c603e60da4967575a9f5e4ed30ddf8"' : 'data-target="#xs-components-links-module-ExamplesModule-22c603e60da4967575a9f5e4ed30ddf8"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ExamplesModule-22c603e60da4967575a9f5e4ed30ddf8"' :
                                            'id="xs-components-links-module-ExamplesModule-22c603e60da4967575a9f5e4ed30ddf8"' }>
                                            <li class="link">
                                                <a href="components/ExamplesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ExamplesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormsPlaygroundComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormsPlaygroundComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/SharedModule.html" data-type="entity-link">SharedModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#components-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' : 'data-target="#xs-components-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' :
                                            'id="xs-components-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                            <li class="link">
                                                <a href="components/AccordionComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AccordionComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ActionButtonComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ActionButtonComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ActionButtonsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ActionButtonsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppFormComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AppFormComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CardComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CardComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CardDeckComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CardDeckComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DateFilterComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">DateFilterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DropdownFloatingFilterComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">DropdownFloatingFilterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormButtonComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormButtonComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormButtonGroupComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormButtonGroupComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormCheckboxComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormCheckboxComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormCheckboxListComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormCheckboxListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormDateComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormDateComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormFieldErrorComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormFieldErrorComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormFileComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormFileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormFilePathComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormFilePathComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormInputComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormInputComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormInputGroupComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormInputGroupComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormRadioListComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormRadioListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormSelectComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormSelectComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormTextareaComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormTextareaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormTimeComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormTimeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">GridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImageResizerComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ImageResizerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ListComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LoadingComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">LoadingComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LoginComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">LoginComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LoginMenuComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">LoginMenuComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LogoutComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">LogoutComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ModalComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ModalComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PageHeadingComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">PageHeadingComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SearchInputComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SearchInputComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ToastComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ToastComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ToggleSwitchComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ToggleSwitchComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TypeaheadComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">TypeaheadComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                        'data-target="#directives-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' : 'data-target="#xs-directives-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                        <span class="icon ion-md-code-working"></span>
                                        <span>Directives</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="directives-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' :
                                        'id="xs-directives-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                        <li class="link">
                                            <a href="directives/AppFileInputDirective.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules">AppFileInputDirective</a>
                                        </li>
                                        <li class="link">
                                            <a href="directives/FieldColorValidationDirective.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules">FieldColorValidationDirective</a>
                                        </li>
                                        <li class="link">
                                            <a href="directives/FormFieldDirective.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules">FormFieldDirective</a>
                                        </li>
                                        <li class="link">
                                            <a href="directives/ModalTemplateDirective.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules">ModalTemplateDirective</a>
                                        </li>
                                    </ul>
                                </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                        'data-target="#injectables-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' : 'data-target="#xs-injectables-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' :
                                        'id="xs-injectables-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                        <li class="link">
                                            <a href="injectables/FormsService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>FormsService</a>
                                        </li>
                                    </ul>
                                </li>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#pipes-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' : 'data-target="#xs-pipes-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                            <span class="icon ion-md-add"></span>
                                            <span>Pipes</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="pipes-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' :
                                            'id="xs-pipes-links-module-SharedModule-b1ce65b1df4e03203a36a7b0fc956b0b"' }>
                                            <li class="link">
                                                <a href="pipes/GroupByPipe.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">GroupByPipe</a>
                                            </li>
                                            <li class="link">
                                                <a href="pipes/SafePipe.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SafePipe</a>
                                            </li>
                                            <li class="link">
                                                <a href="pipes/TranslatePipe.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">TranslatePipe</a>
                                            </li>
                                            <li class="link">
                                                <a href="pipes/UppercasePipe.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">UppercasePipe</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ShopModule.html" data-type="entity-link">ShopModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#components-links-module-ShopModule-121f438e1f3cd308e29f1acb1946a562"' : 'data-target="#xs-components-links-module-ShopModule-121f438e1f3cd308e29f1acb1946a562"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ShopModule-121f438e1f3cd308e29f1acb1946a562"' :
                                            'id="xs-components-links-module-ShopModule-121f438e1f3cd308e29f1acb1946a562"' }>
                                            <li class="link">
                                                <a href="components/CustomersComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CustomersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProductsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ProductsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ShopComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ShopComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/SignalrModule.html" data-type="entity-link">SignalrModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#components-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' : 'data-target="#xs-components-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' :
                                            'id="xs-components-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' }>
                                            <li class="link">
                                                <a href="components/ChatComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ChatComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MoveShapeComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MoveShapeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SignalrComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SignalrComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                        'data-target="#directives-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' : 'data-target="#xs-directives-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' }>
                                        <span class="icon ion-md-code-working"></span>
                                        <span>Directives</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="directives-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' :
                                        'id="xs-directives-links-module-SignalrModule-2072108e39c48490531c43c69c5d549d"' }>
                                        <li class="link">
                                            <a href="directives/DraggableDirective.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules">DraggableDirective</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                </ul>
                </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#components-links"' :
                            'data-target="#xs-components-links"' }>
                            <span class="icon ion-md-cog"></span>
                            <span>Components</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="components-links"' : 'id="xs-components-links"' }>
                            <li class="link">
                                <a href="components/AccordionComponent.html" data-type="entity-link">AccordionComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ActionButtonComponent.html" data-type="entity-link">ActionButtonComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ActionButtonsComponent.html" data-type="entity-link">ActionButtonsComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/AppFormComponent.html" data-type="entity-link">AppFormComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CardComponent.html" data-type="entity-link">CardComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CardDeckComponent.html" data-type="entity-link">CardDeckComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/DateFilterComponent.html" data-type="entity-link">DateFilterComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/DropdownFloatingFilterComponent.html" data-type="entity-link">DropdownFloatingFilterComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FooterComponent.html" data-type="entity-link">FooterComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormButtonComponent.html" data-type="entity-link">FormButtonComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormButtonGroupComponent.html" data-type="entity-link">FormButtonGroupComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormCheckboxComponent.html" data-type="entity-link">FormCheckboxComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormCheckboxListComponent.html" data-type="entity-link">FormCheckboxListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormDateComponent.html" data-type="entity-link">FormDateComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormFieldErrorComponent.html" data-type="entity-link">FormFieldErrorComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormFileComponent.html" data-type="entity-link">FormFileComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormFilePathComponent.html" data-type="entity-link">FormFilePathComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormInputComponent.html" data-type="entity-link">FormInputComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormInputGroupComponent.html" data-type="entity-link">FormInputGroupComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormRadioListComponent.html" data-type="entity-link">FormRadioListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormSelectComponent.html" data-type="entity-link">FormSelectComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormTextareaComponent.html" data-type="entity-link">FormTextareaComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FormTimeComponent.html" data-type="entity-link">FormTimeComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/GridComponent.html" data-type="entity-link">GridComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/HeaderComponent.html" data-type="entity-link">HeaderComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ImageResizerComponent.html" data-type="entity-link">ImageResizerComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ListComponent.html" data-type="entity-link">ListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/LoadingComponent.html" data-type="entity-link">LoadingComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/LoginComponent.html" data-type="entity-link">LoginComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/LoginMenuComponent.html" data-type="entity-link">LoginMenuComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/LogoutComponent.html" data-type="entity-link">LogoutComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ModalComponent.html" data-type="entity-link">ModalComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PageHeadingComponent.html" data-type="entity-link">PageHeadingComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PrivacyComponent.html" data-type="entity-link">PrivacyComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/SearchInputComponent.html" data-type="entity-link">SearchInputComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ToastComponent.html" data-type="entity-link">ToastComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ToggleSwitchComponent.html" data-type="entity-link">ToggleSwitchComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/TypeaheadComponent.html" data-type="entity-link">TypeaheadComponent</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#directives-links"' :
                                'data-target="#xs-directives-links"' }>
                                <span class="icon ion-md-code-working"></span>
                                <span>Directives</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="directives-links"' : 'id="xs-directives-links"' }>
                                <li class="link">
                                    <a href="directives/AppFileInputDirective.html" data-type="entity-link">AppFileInputDirective</a>
                                </li>
                                <li class="link">
                                    <a href="directives/FieldColorValidationDirective.html" data-type="entity-link">FieldColorValidationDirective</a>
                                </li>
                                <li class="link">
                                    <a href="directives/FormFieldDirective.html" data-type="entity-link">FormFieldDirective</a>
                                </li>
                                <li class="link">
                                    <a href="directives/ModalTemplateDirective.html" data-type="entity-link">ModalTemplateDirective</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#classes-links"' :
                            'data-target="#xs-classes-links"' }>
                            <span class="icon ion-ios-paper"></span>
                            <span>Classes</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="classes-links"' : 'id="xs-classes-links"' }>
                            <li class="link">
                                <a href="classes/ApplicationDataViewModel.html" data-type="entity-link">ApplicationDataViewModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/CategoriesListVm.html" data-type="entity-link">CategoriesListVm</a>
                            </li>
                            <li class="link">
                                <a href="classes/CategoryDto.html" data-type="entity-link">CategoryDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/CreateCustomerCommand.html" data-type="entity-link">CreateCustomerCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/CreateProductCommand.html" data-type="entity-link">CreateProductCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/CulturesDisplayViewModel.html" data-type="entity-link">CulturesDisplayViewModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/CustomerDetailVm.html" data-type="entity-link">CustomerDetailVm</a>
                            </li>
                            <li class="link">
                                <a href="classes/CustomerLookupDto.html" data-type="entity-link">CustomerLookupDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/CustomersListVm.html" data-type="entity-link">CustomersListVm</a>
                            </li>
                            <li class="link">
                                <a href="classes/EmployeeDetailVm.html" data-type="entity-link">EmployeeDetailVm</a>
                            </li>
                            <li class="link">
                                <a href="classes/EmployeeLookupDto.html" data-type="entity-link">EmployeeLookupDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/EmployeeTerritoryDto.html" data-type="entity-link">EmployeeTerritoryDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/EnvironmentInformation.html" data-type="entity-link">EnvironmentInformation</a>
                            </li>
                            <li class="link">
                                <a href="classes/JwtHelperService.html" data-type="entity-link">JwtHelperService</a>
                            </li>
                            <li class="link">
                                <a href="classes/MockAppService.html" data-type="entity-link">MockAppService</a>
                            </li>
                            <li class="link">
                                <a href="classes/MockAuthService.html" data-type="entity-link">MockAuthService</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProblemDetails.html" data-type="entity-link">ProblemDetails</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProductDetailVm.html" data-type="entity-link">ProductDetailVm</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProductDto.html" data-type="entity-link">ProductDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProductsListVm.html" data-type="entity-link">ProductsListVm</a>
                            </li>
                            <li class="link">
                                <a href="classes/SwaggerException.html" data-type="entity-link">SwaggerException</a>
                            </li>
                            <li class="link">
                                <a href="classes/UpdateCustomerCommand.html" data-type="entity-link">UpdateCustomerCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/UpdateProductCommand.html" data-type="entity-link">UpdateProductCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/UpsertCategoryCommand.html" data-type="entity-link">UpsertCategoryCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/UpsertEmployeeCommand.html" data-type="entity-link">UpsertEmployeeCommand</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#injectables-links"' :
                                'data-target="#xs-injectables-links"' }>
                                <span class="icon ion-md-arrow-round-down"></span>
                                <span>Injectables</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="injectables-links"' : 'id="xs-injectables-links"' }>
                                <li class="link">
                                    <a href="injectables/AppClient.html" data-type="entity-link">AppClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppService.html" data-type="entity-link">AppService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AuthorizeService.html" data-type="entity-link">AuthorizeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CategoriesClient.html" data-type="entity-link">CategoriesClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfigService.html" data-type="entity-link">ConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CustomDateFormatter.html" data-type="entity-link">CustomDateFormatter</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CustomersClient.html" data-type="entity-link">CustomersClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CustomNgbDateNativeUTCAdapter.html" data-type="entity-link">CustomNgbDateNativeUTCAdapter</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DataService.html" data-type="entity-link">DataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EmployeesClient.html" data-type="entity-link">EmployeesClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FieldBaseComponent.html" data-type="entity-link">FieldBaseComponent</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FormsService.html" data-type="entity-link">FormsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GlobalErrorHandler.html" data-type="entity-link">GlobalErrorHandler</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LoadingService.html" data-type="entity-link">LoadingService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ModalService.html" data-type="entity-link">ModalService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ModalStateService.html" data-type="entity-link">ModalStateService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProductsClient.html" data-type="entity-link">ProductsClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ToastService.html" data-type="entity-link">ToastService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TranslatePipe.html" data-type="entity-link">TranslatePipe</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UtilsService.html" data-type="entity-link">UtilsService</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#interceptors-links"' :
                            'data-target="#xs-interceptors-links"' }>
                            <span class="icon ion-ios-swap"></span>
                            <span>Interceptors</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="interceptors-links"' : 'id="xs-interceptors-links"' }>
                            <li class="link">
                                <a href="interceptors/AuthInterceptor.html" data-type="entity-link">AuthInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/JwtInterceptor.html" data-type="entity-link">JwtInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/LoadingInterceptor.html" data-type="entity-link">LoadingInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/TimingInterceptor.html" data-type="entity-link">TimingInterceptor</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#guards-links"' :
                            'data-target="#xs-guards-links"' }>
                            <span class="icon ion-ios-lock"></span>
                            <span>Guards</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="guards-links"' : 'id="xs-guards-links"' }>
                            <li class="link">
                                <a href="guards/AuthorizeGuard.html" data-type="entity-link">AuthorizeGuard</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#interfaces-links"' :
                            'data-target="#xs-interfaces-links"' }>
                            <span class="icon ion-md-information-circle-outline"></span>
                            <span>Interfaces</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? ' id="interfaces-links"' : 'id="xs-interfaces-links"' }>
                            <li class="link">
                                <a href="interfaces/ApplicationPathsType.html" data-type="entity-link">ApplicationPathsType</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownFloatingFilterParams.html" data-type="entity-link">DropdownFloatingFilterParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FailureAuthenticationResult.html" data-type="entity-link">FailureAuthenticationResult</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Field.html" data-type="entity-link">Field</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Field-1.html" data-type="entity-link">Field</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FieldOption.html" data-type="entity-link">FieldOption</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FileResponse.html" data-type="entity-link">FileResponse</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ForgotPassword.html" data-type="entity-link">ForgotPassword</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GridColumn.html" data-type="entity-link">GridColumn</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IAccordionItem.html" data-type="entity-link">IAccordionItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IAppClient.html" data-type="entity-link">IAppClient</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IApplicationDataViewModel.html" data-type="entity-link">IApplicationDataViewModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICard.html" data-type="entity-link">ICard</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICardEvent.html" data-type="entity-link">ICardEvent</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICategoriesClient.html" data-type="entity-link">ICategoriesClient</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICategoriesListVm.html" data-type="entity-link">ICategoriesListVm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICategoryDto.html" data-type="entity-link">ICategoryDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICreateCustomerCommand.html" data-type="entity-link">ICreateCustomerCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICreateProductCommand.html" data-type="entity-link">ICreateProductCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICulturesDisplayViewModel.html" data-type="entity-link">ICulturesDisplayViewModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICustomerDetailVm.html" data-type="entity-link">ICustomerDetailVm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICustomerLookupDto.html" data-type="entity-link">ICustomerLookupDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICustomersClient.html" data-type="entity-link">ICustomersClient</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICustomersListVm.html" data-type="entity-link">ICustomersListVm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IEmployeeDetailVm.html" data-type="entity-link">IEmployeeDetailVm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IEmployeeLookupDto.html" data-type="entity-link">IEmployeeLookupDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IEmployeesClient.html" data-type="entity-link">IEmployeesClient</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IEmployeeTerritoryDto.html" data-type="entity-link">IEmployeeTerritoryDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IEnableAuthenticatorModel.html" data-type="entity-link">IEnableAuthenticatorModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IEnvironmentInformation.html" data-type="entity-link">IEnvironmentInformation</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IFieldConfig.html" data-type="entity-link">IFieldConfig</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IFieldConfig-1.html" data-type="entity-link">IFieldConfig</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IListItem.html" data-type="entity-link">IListItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IModalOptions.html" data-type="entity-link">IModalOptions</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/INavigationState.html" data-type="entity-link">INavigationState</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/INavigationState-1.html" data-type="entity-link">INavigationState</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IOption.html" data-type="entity-link">IOption</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IProblemDetails.html" data-type="entity-link">IProblemDetails</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IProductDetailVm.html" data-type="entity-link">IProductDetailVm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IProductDto.html" data-type="entity-link">IProductDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IProductsClient.html" data-type="entity-link">IProductsClient</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IProductsListVm.html" data-type="entity-link">IProductsListVm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ISocialLogins.html" data-type="entity-link">ISocialLogins</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ITwoFactorModel.html" data-type="entity-link">ITwoFactorModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUpdateCustomerCommand.html" data-type="entity-link">IUpdateCustomerCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUpdateProductCommand.html" data-type="entity-link">IUpdateProductCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUpsertCategoryCommand.html" data-type="entity-link">IUpsertCategoryCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUpsertEmployeeCommand.html" data-type="entity-link">IUpsertEmployeeCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUser.html" data-type="entity-link">IUser</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/KeyValuePair.html" data-type="entity-link">KeyValuePair</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ModalOptions.html" data-type="entity-link">ModalOptions</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NavItem.html" data-type="entity-link">NavItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PageNav.html" data-type="entity-link">PageNav</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RedirectAuthenticationResult.html" data-type="entity-link">RedirectAuthenticationResult</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ResetPasswordToken.html" data-type="entity-link">ResetPasswordToken</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SuccessAuthenticationResult.html" data-type="entity-link">SuccessAuthenticationResult</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#pipes-links"' :
                                'data-target="#xs-pipes-links"' }>
                                <span class="icon ion-md-add"></span>
                                <span>Pipes</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="pipes-links"' : 'id="xs-pipes-links"' }>
                                <li class="link">
                                    <a href="pipes/GroupByPipe.html" data-type="entity-link">GroupByPipe</a>
                                </li>
                                <li class="link">
                                    <a href="pipes/SafePipe.html" data-type="entity-link">SafePipe</a>
                                </li>
                                <li class="link">
                                    <a href="pipes/TranslatePipe.html" data-type="entity-link">TranslatePipe</a>
                                </li>
                                <li class="link">
                                    <a href="pipes/UppercasePipe.html" data-type="entity-link">UppercasePipe</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#miscellaneous-links"'
                            : 'data-target="#xs-miscellaneous-links"' }>
                            <span class="icon ion-ios-cube"></span>
                            <span>Miscellaneous</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="miscellaneous-links"' : 'id="xs-miscellaneous-links"' }>
                            <li class="link">
                                <a href="miscellaneous/enumerations.html" data-type="entity-link">Enums</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/functions.html" data-type="entity-link">Functions</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/typealiases.html" data-type="entity-link">Type aliases</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/variables.html" data-type="entity-link">Variables</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <a data-type="chapter-link" href="routes.html"><span class="icon ion-ios-git-branch"></span>Routes</a>
                        </li>
                    <li class="chapter">
                        <a data-type="chapter-link" href="coverage.html"><span class="icon ion-ios-stats"></span>Documentation coverage</a>
                    </li>
                    <li class="divider"></li>
                    <li class="copyright">
                        Documentation generated using <a href="https://compodoc.app/" target="_blank">
                            <img data-src="images/compodoc-vectorise.png" class="img-responsive" data-type="compodoc-logo">
                        </a>
                    </li>
            </ul>
        </nav>
        `);
        this.innerHTML = tp.strings;
    }
});