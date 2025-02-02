import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { handlePaymentCallback } from "../../../State/Payment/Action";
import { Alert, AlertTitle, Divider, Grid } from "@mui/material";
import OrderTracker from "../order/OrderTracker";
import { useParams } from "react-router";
import { getOrderById } from "../../../State/Order/Action";
import AddressCard from "../addressCard/AddressCard";

const PaymentSuccess = () => {
  const dispatch = useDispatch();
  const { order, payment } = useSelector((store) => store);
  const { orderId } = useParams();

  const storedUrl = localStorage.getItem("paymentUrl");
  useEffect(() => {
    if (!payment.url) {
      // Fetch URL from localStorage if not available in Redux state
      if (!storedUrl) {
        console.error("Payment URL not found.");
      } else {
        console.log("Payment URL from localStorage:", storedUrl);
      }
    } else {
      console.log("Payment URL from Redux:", paymentUrl);
    }
    const extractSessionId = (url) => {
      // Extract session ID from the URL
      const pattern = /\/cs_test_[^#]+/; // Matches the session ID pattern
      const match = url.match(pattern);
      if (match) {
        return match[0].replace("/", ""); // Remove the leading slash
      }
      return null;
    };

    console.log(payment);
    const sessionId = extractSessionId(storedUrl);
    console.log("Session Id: ", sessionId);
    dispatch(handlePaymentCallback(sessionId));
  }, [dispatch, payment.url]);

  useEffect(() => {
    dispatch(getOrderById(orderId));
    console.log("Order from success: ", order);
  }, [orderId]);

  return (
    <div className="px-2 lg:px-36">
      <div className="flex flex-col justify-center items-center">
        <Alert
          variant="filled"
          severity="success"
          sx={{ mb: 6, width: "fit-content" }}
        >
          <AlertTitle>Payment Success...</AlertTitle>
          Congratulations Your Order Get Placed
        </Alert>
      </div>
      <OrderTracker activeStep={1} />
      <Grid container className="space-y-5 py-5 pt-20">
        {order.order?.orderItems.map((item) => (
          <Grid
            container
            item
            className="space-y-5 border p-4 rounded-sm"
            sx={{ alignItems: "center", justifyContent: "space-between" }}
          >
            <Grid item xs={6}>
              <div className="flex items-center">
                <img className="w-[5rem] h-[5rem] object-cover object-top" src={item.product.imageUrl} alt="" />

                <div className="ml-5">
                  <p>{item.product.title}</p>
                  <div className="opacity-60 text-xs font-semibold space-x-5">
                    <span>Color: {item.product.color}</span>
                    <span>Size: {item.size}</span>
                  </div>
                  <p>Seller: {item.product.brand}</p>
                  <p>Rs.{item.price}</p>
                </div>
              </div>
            </Grid>
            <Divider orientation="vertical" flexItem />
            <Grid item>
              <AddressCard address={order.order?.shippingAddress} />
            </Grid>
          </Grid>
        ))}
      </Grid>
    </div>
  );
};

export default PaymentSuccess;
