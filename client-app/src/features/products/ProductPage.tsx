import axios from "axios";
import { observer } from "mobx-react-lite";
import React, { Fragment, useEffect, useState } from "react";
import { Grid, Header, List } from "semantic-ui-react";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";
import { useStore } from "../../app/stores/store";

export default observer(function ProductPage() {
    const { productStore } = useStore();
    const { loadProducts, productRegistry } = productStore;
    const [ products, setProducts] = useState<Product[]>([]);


    // useEffect(() => {
    //     if (productRegistry.size <= 1) loadProducts();
    // }, [productRegistry.size, loadProducts])

    // if (productStore.loadingInitial) return <LoadingComponent content='Loading products' />

    useEffect(() => {
        axios.get<Product[]>('http://localhost:7000/api/product').then(response => {
            setProducts(response.data);
        })
    }, [])

    return (
        <>
            <List>
                {products.map(product => (
                    <List.Item key={product.element}>
                        {product.element}
                    </List.Item>
                ))}
            </List>
        </>
    )

})