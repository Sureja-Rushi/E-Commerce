import React from "react";
import Navbar from "./customers/components/navigation/Navbar";
import HomePage from "./customers/pages/home/HomePage";
import Footer from "./customers/components/footer/Footer";
import Product from "./customers/components/product/Product";
import ProductDetails from "./customers/components/productdetails/ProductDetails";
import Cart from "./customers/components/cart/Cart";
import Checkout from "./customers/components/checkout/Checkout";
import Order from "./customers/components/order/Order";
import OrderDetails from "./customers/components/order/OrderDetails";
import { Route, Routes } from "react-router";
import CustomerRouters from "./routers/CustomerRouters";

function App() {
  return (
    <div>
      <Routes>
        <Route path="/*" element={<CustomerRouters />}></Route>
      </Routes>
      {/* <Navbar /> */}
      {/* <div> */}
        {/* <HomePage /> */}
        {/* <Product /> */}
        {/* <ProductDetails /> */}
        {/* <Cart /> */}
        {/* <Checkout /> */}
        {/* <Order /> */}
        {/* <OrderDetails /> */}
      {/* </div> */}
      {/* <Footer /> */}
    </div>
  );
}

export default App;
