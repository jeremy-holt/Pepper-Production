import { Router, RouterConfiguration } from "aurelia-router";

export class App {
    router: Router;
    message:string;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Pepper Calculator";
    
    config.map([
      { route: '', moduleId: 'components/home/home', nav: true, title: 'Home', name: 'home' },
      { route: 'calculator', moduleId: 'components/calculator/calculator-main', name: 'calculator', nav: true, title: 'Calculator' },
      { route: 'farm-inputs-main', moduleId: 'components/dictionary/farm-inputs-main', name: 'Dictionary', nav: true, title: 'Dictionary' }
    ]);

    this.router = router;
  }
}
