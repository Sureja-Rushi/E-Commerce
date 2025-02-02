import React, { useEffect } from "react";
import AddressCard from "../addressCard/AddressCard";
import CartItem from "../cart/CartItem";
import { Button, Divider } from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import { getOrderById } from "../../../State/Order/Action";
import { useLocation } from "react-router";
import { initiatePayment } from "../../../State/Payment/Action";

const OrderSummary = () => {
  const dispatch = useDispatch();
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const orderId = searchParams.get("order_id");
  const { order } = useSelector((store) => store);

  useEffect(() => {
    dispatch(getOrderById(orderId));
    console.log("Order from summ: ", order.order);
  }, [orderId]);

  const handleCheckout = () => {
    dispatch(initiatePayment(orderId));
  }

  return (
    <div>
      <div className="p-5 shadow-lg rounded-md border">
        <AddressCard address={order.order?.shippingAddress} />
      </div>

      <div>
        <div className="lg:grid grid-cols-3 lg:mt-5 relative">
          <div className="col-span-2">
            {order.order?.orderItems.map((item) => (
              <CartItem item={item} disable={false} />
            ))}
          </div>
          <div className="px-5 sticky top-0 h-[100vh] mt-5 lg:mt-0">
            <div className="border p-2">
              <p className="uppercase font-bold opacity-70 pb-4 flex justify-center items-center">
                Price Details
              </p>
              <Divider />
              <div className="space-y-3 font-semibold mb-10">
                <div className="flex justify-between pt-3 text-black">
                  <span>Price</span>
                  <span>Rs.{order.order?.totalPrice}</span>
                </div>
                <div className="flex justify-between pt-3 text-black">
                  <span>Discount</span>
                  <span className="text-green-600">-Rs.{order.order?.discount}</span>
                </div>
                <div className="flex justify-between pt-3 text-black">
                  <span>Delivery Charge</span>
                  <span className="text-green-600">Free</span>
                </div>
                <div className="flex justify-between pt-3 text-black font-bold">
                  <span>Total Amount</span>
                  <span className="text-green-600">Rs.{order.order?.totalDiscountedPrice}</span>
                </div>
              </div>
              <Button
                variant="contained"
                className="w-full"
                sx={{ px: "2.5rem", py: ".5rem", bgcolor: "#9155fd" }}
                onClick={handleCheckout}
              >
                Confirm Order
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderSummary;
