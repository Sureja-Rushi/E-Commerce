import React, { Fragment, useState } from "react";
import { useDispatch } from "react-redux";
import { createProduct } from "../../State/Product/Action";
import {
  Button,
  FormControl,
  Grid,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  Typography,
} from "@mui/material";

const initialSizes = [
  { sizeName: "S", quantity: 0 },
  { sizeName: "M", quantity: 0 },
  { sizeName: "L", quantity: 0 },
  { sizeName: "XL", quantity: 0 },
  { sizeName: "XXL", quantity: 0 },
];

const CreateProductForm = () => {
  const [productData, setProductData] = useState({
    imageUrl: "",
    brand: "",
    title: "",
    color: "",
    discountedPrice: "",
    price: "",
    discountPercent: "",
    sizes: initialSizes,
    quantity: "",
    grandParentCategoryName: "",
    parentCategoryName: "",
    categoryName: "",
    description: "",
  });
  const dispatch = useDispatch();
  const jwt = localStorage.getItem("AuthToken");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProductData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSizeChange = (e, index) => {
    let { name, value } = e.target;
    name === "size_quantity" ? (name = "quantity") : (name = e.target), name;

    const sizes = [...productData.sizes];
    sizes[index][name] = value;
    setProductData((prevState) => ({
      ...prevState,
      size: sizes,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Add Product: ", productData);
    dispatch(createProduct(productData));
  };

  return (
    <div>
      <div className="createProductContainer p-10">
        <Typography
          variant="h3"
          sx={{ textAlign: "center" }}
          className="py-10 text-center"
        >
          Add New Product
        </Typography>
        <form
          onSubmit={handleSubmit}
          className="min-h-screen createProductContainer"
        >
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="image URL"
                name="imageUrl"
                value={productData.imageUrl}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Brand"
                name="brand"
                value={productData.brand}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Title"
                name="title"
                value={productData.title}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Color"
                name="color"
                value={productData.color}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Quantity"
                name="quantity"
                value={productData.quantity}
                onChange={handleChange}
                type="number"
              />
            </Grid>

            <Grid item xs={12} sm={4}>
              <TextField
                fullWidth
                label="Price"
                name="price"
                value={productData.price}
                onChange={handleChange}
                type="number"
              />
            </Grid>

            <Grid item xs={12} sm={4}>
              <TextField
                fullWidth
                label="Discounted Price"
                name="discountedPrice"
                value={productData.discountedPrice}
                onChange={handleChange}
                type="number"
              />
            </Grid>

            <Grid item xs={12} sm={4}>
              <TextField
                fullWidth
                label="Discount Percentage"
                name="discountPercent"
                value={productData.discountPercent}
                onChange={handleChange}
                type="number"
              />
            </Grid>

            <Grid item xs={6} sm={4}>
              <FormControl fullWidth>
                <InputLabel>Grand Parent Category</InputLabel>
                <Select
                  name="grandParentCategoryName"
                  value={productData.grandParentCategoryName}
                  onChange={handleChange}
                  label="Grand Parent Category"
                >
                  <MenuItem value="men">Men</MenuItem>
                  <MenuItem value="women">Women</MenuItem>
                  <MenuItem value="kids">Kids</MenuItem>
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={6} sm={4}>
              <FormControl fullWidth>
                <InputLabel>Parent Category</InputLabel>
                <Select
                  name="parentCategoryName"
                  value={productData.parentCategoryName}
                  onChange={handleChange}
                  label="Parent Category"
                >
                  <MenuItem value="clothing">Clothing</MenuItem>
                  <MenuItem value="accessories">Accessories</MenuItem>
                  <MenuItem value="brands">Brands</MenuItem>
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={6} sm={4}>
              <FormControl fullWidth>
                <InputLabel>Category</InputLabel>
                <Select
                  name="categoryName"
                  value={productData.categoryName}
                  onChange={handleChange}
                  label="Category"
                >
                  <MenuItem value="top">Tops</MenuItem>
                  <MenuItem value="women_dress">Cresses</MenuItem>
                  <MenuItem value="t-shirts">T-Shirts</MenuItem>
                  <MenuItem value="saree">Saree</MenuItem>
                  <MenuItem value="lengha_choli">Lengha Choli</MenuItem>
                  <MenuItem value="mens_kurta">Mens Kurta</MenuItem>
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                id="outlined-multiline-static"
                label="Description"
                multiline
                name="description"
                rows={3}
                onChange={handleChange}
                value={productData.description}
              />
            </Grid>
            {productData.sizes.map((size, index) => (
              <Grid container item spacing={3}>
                <Grid item xs={12} sm={6}>
                  <TextField
                    label="Size Name"
                    name="name"
                    value={size.sizeName}
                    onChange={(event) => handleSizeChange(event, index)}
                    required
                    fullWidth
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    label="Quantity"
                    name="size_quantity"
                    type="number"
                    onChange={(event) => handleSizeChange(event, index)}
                    required
                    fullWidth
                  />
                </Grid>
              </Grid>
            ))}
            <Grid item xs={12}>
              <Button variant="contained" sx={{p:1.8}} className="py-20" size="large" type="submit">
                Add New Product
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </div>
  );
};

export default CreateProductForm;
