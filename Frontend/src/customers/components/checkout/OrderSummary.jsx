import React from "react";
import AddressCard from "../addressCard/AddressCard";
import CartItem from "../cart/CartItem";
import { Button, Divider } from "@mui/material";

const OrderSummary = () => {
  return (
    <div>
      <div className="p-5 shadow-lg rounded-md border">
        <AddressCard />
      </div>

      <div>
        <div className="lg:grid grid-cols-3 lg:mt-5 relative">
          <div className="col-span-2">
            {[1, 1, 1, 1, 1].map((item) => (
              <CartItem />
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
                  <span>Rs.4697</span>
                </div>
                <div className="flex justify-between pt-3 text-black">
                  <span>Discount</span>
                  <span className="text-green-600">-Rs.3419</span>
                </div>
                <div className="flex justify-between pt-3 text-black">
                  <span>Delivery Charge</span>
                  <span className="text-green-600">Free</span>
                </div>
                <div className="flex justify-between pt-3 text-black font-bold">
                  <span>Total Amount</span>
                  <span className="text-green-600">Rs.1278</span>
                </div>
              </div>
              <Button
                variant="contained"
                className="w-full"
                sx={{ px: "2.5rem", py: ".5rem", bgcolor: "#9155fd" }}
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
