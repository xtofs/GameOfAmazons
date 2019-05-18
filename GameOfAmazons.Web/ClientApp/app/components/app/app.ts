import { Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'GameOfAmazons.Web';
        config.map([{
            route: [ 'home' ],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../home/home'),
            nav: true,
            title: 'Home'        
        }, {
            route: ['', 'game'],
            name: 'game',
            settings: { icon: 'th-list' },
            moduleId: PLATFORM.moduleName('../game/game'),
            nav: true,
            title: 'game'
        }]);

        this.router = router;
    }
}
