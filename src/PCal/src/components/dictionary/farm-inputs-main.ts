import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient)
export class FarmInputsMain {

  farmProducts: Array<any>;

  constructor() {
    let httpClient = new HttpClient();
    httpClient.configure(config => {
      config.useStandardConfiguration()
        .withBaseUrl("api/")
        .withDefaults({
          credentials: 'same-origin',
          headers: {
            'X-Requested-With': 'Fetch'
          }
        })
    });

    httpClient.fetch("farmProducts")
      .then(response => response.json())
      .then(data => {
        this.farmProducts = data;
        console.log(this.farmProducts);
      });

  }
}
