import React from "react";
import { Route, Routes } from "react-router";
import HomePage from "../customers/pages/home/HomePage";
import Cart from "../customers/components/cart/Cart";
import Navbar from "../customers/components/navigation/Navbar";
import Footer from "../customers/components/footer/Footer";
import Product from "../customers/components/product/Product";
import ProductDetails from "../customers/components/productdetails/ProductDetails";
import Checkout from "../customers/components/checkout/Checkout";
import Order from "../customers/components/order/Order";
import OrderDetails from "../customers/components/order/OrderDetails";
import PaymentSuccess from "../customers/components/Payment/PaymentSuccess";

const CustomerRouters = () => {
  return (
    <div>
      <div>
        <Navbar />
      </div>

      <Routes>
      <Route path='/login' element={<HomePage />} ></Route>
      <Route path='/register' element={<HomePage />} ></Route>

        <Route path="/" element={<HomePage />}></Route>
        <Route path="/cart" element={<Cart />}></Route>
        <Route
          path="/:levelOne/:levelTwo/:levelThree"
          element={<Product />}
        ></Route>
        <Route path="/product/:productId" element={<ProductDetails />}></Route>
        <Route path="/checkout" element={<Checkout />}></Route>
        <Route path="/account/order" element={<Order />}></Route>
        <Route
          path="/account/order/:orderId"
          element={<OrderDetails />}
        ></Route>
        <Route path="/payment/:orderId" element={<PaymentSuccess />} ></Route>
      </Routes>
      <div>
        <Footer />
      </div>
    </div>
  );
};

export default CustomerRouters;
