import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient)
export class FarmProductsService {
    constructor(private httpClient: HttpClient) {

    }

    getFarmProductsList() {
        return this.httpClient.fetch("farmProducts")
            .then(
                response => response.json(),
                response => response.text())
            .then(data => {                
                for (let item of data) {
                    item.coverageText = `.   bbbb ${item.coverageText}`;
                }
                return data;
            });
    }
}