import {Aurelia} from "aurelia-framework"
import environment from "./environment";
import {HttpClient} from "aurelia-fetch-client";

//Configure Bluebird Promises.
(Promise as any).config({
    longStackTraces: environment.debug,
    warnings: {
        wForgottenReturn: false
    }
});

export function configure(aurelia: Aurelia) {
    aurelia.use
        .standardConfiguration()        
        .feature("resources");

    const container = aurelia.container;

    const http = new HttpClient();

    http.configure(config => {
        config
            .useStandardConfiguration()
            .withBaseUrl("api/")
            .withDefaults({
                credentials: "same-origin",
                headers: {
                    'X-Requested-With': "Fetch"
                }
            })
            .withInterceptor({
                request(request) {
                    console.log(`Requesting ${request.method} ${request.url}`);
                    return request;
                },
                response(response) {
                    console.log(`Received ${response.status} ${response.url}`);
                    return response;
                }
            });
    });

    container.registerInstance(HttpClient, http);

    if (environment.debug) {
        aurelia.use.developmentLogging();
    }

    if (environment.testing) {
        aurelia.use.plugin("aurelia-testing");
    }

    aurelia.start().then(() => aurelia.setRoot());
}