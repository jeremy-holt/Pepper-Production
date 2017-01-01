import { inject } from "aurelia-framework";
import {FarmProductsService} from "../../services/FarmProductsService";


@inject(FarmProductsService)
export class FarmInputsMain {

    farmProducts: Array<any>;

    constructor(private service: FarmProductsService) {

    }

    activate() {
        return this.service.getFarmProductsList()
            .then(data => {
                this.farmProducts = data;
                console.log(this.farmProducts);
            });
    }

    
}