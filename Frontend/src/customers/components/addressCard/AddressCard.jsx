import React from "react";

const capitalizeWords = (str) => {
  if (!str) return '';
  return str
    .split(' ')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ');
};

const AddressCard = ({ address }) => {
  return (
    <div>
      <div className="space-y-3">
        <p className="font-semibold">
          {capitalizeWords(address?.firstName) + " " + capitalizeWords(address?.lastName)}
        </p>
        <p>
          {capitalizeWords(address?.street) +
            ", " +
            capitalizeWords(address?.city) +
            ", " +
            capitalizeWords(address?.state) +
            ", " +
            capitalizeWords(address?.zipCode)}
        </p>
        <div className="space-y-1">
          <p className="font-semibold">Contact Number</p>
          <p>{address?.contactNumber}</p>
        </div>
      </div>
    </div>
  );
};

export default AddressCard;
