import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Product } from "../models/product";

export default class ProductStore {
    productRegistry = new Map<string, Product>();
    loading = false;
    loadingInitial = false;

    constructor() {
        makeAutoObservable(this)
    }


    loadProducts = async () => {
        this.loadingInitial = true;
        try {
            const products = await agent.Products.list();
            products.forEach(product => {
                this.setProduct(product);
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
        }
    }

    private setProduct = (product: Product) => {
        this.productRegistry.set(product.id, product);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}