import {
  Box,
  CssBaseline,
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import React, { useState } from "react";
import { Route, Routes, useNavigate } from "react-router";
import DashboardRoundedIcon from "@mui/icons-material/DashboardRounded";
import AddShoppingCartRoundedIcon from "@mui/icons-material/AddShoppingCartRounded";
import PersonOutlineRoundedIcon from "@mui/icons-material/PersonOutlineRounded";
import BookmarkBorderRoundedIcon from "@mui/icons-material/BookmarkBorderRounded";
import LibraryAddRoundedIcon from "@mui/icons-material/LibraryAddRounded";
import AccountCircleRoundedIcon from "@mui/icons-material/AccountCircleRounded";
import Dashboard from "./Components/Dashboard";
import CreateProductForm from "./Components/CreateProductForm";
import ProductsTable from "./Components/ProductsTable";
import OrdersTable from "./Components/OrdersTable";
import CustomersTable from "./Components/CustomersTable";

const menu = [
  { name: "Dashboard", path: "/admin", icon: DashboardRoundedIcon },
  {
    name: "Products",
    path: "/admin/products",
    icon: AddShoppingCartRoundedIcon,
  },
  {
    name: "Customers",
    path: "/admin/customers",
    icon: PersonOutlineRoundedIcon,
  },
  { name: "Orders", path: "/admin/orders", icon: BookmarkBorderRoundedIcon },
  { name: "AddProducts", path: "/admin/create", icon: LibraryAddRoundedIcon },
];

const Admin = () => {
  const theme = useTheme();
  const isLargeScreen = useMediaQuery(theme.breakpoints.up("lg"));
  const [sideBarVisible, setSideBarVisible] = useState(false);
  const navigate = useNavigate();

  const drawer = (
    <Box
      sx={{
        overflow: "auto",
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        height: "100%",
        // position: "fixed"
      }}
    >
      {/* {isLargeScreen && <Toolbar />} */}
      <List>
        {menu.map((item, index) => (
          <ListItem
            key={item.name}
            disablePadding
            onClick={() => navigate(item.path)}
          >
            <ListItemButton>
              <ListItemIcon>
                <item.icon /> {/* Render the icon component here */}
              </ListItemIcon>
              <ListItemText>{item.name}</ListItemText>
            </ListItemButton>
          </ListItem>
        ))}
      </List>

      <List>
        <ListItem disablePadding>
          <ListItemButton>
            <ListItemIcon>
              <AccountCircleRoundedIcon />
            </ListItemIcon>
            <ListItemText>Account</ListItemText>
          </ListItemButton>
        </ListItem>
      </List>
    </Box>
  );

  return (
    <div>
      <div className="flex h-[100vh]">
        <CssBaseline />
        <div className="shadow-lg shadow-gray-600 w-[15%] h-full fixed top-0">
          {drawer}
        </div>
        <div className="w-[85%] ml-[15%]">
          <Routes>
            <Route path="/" element={<Dashboard />}></Route>
            <Route path="/create" element={<CreateProductForm />}></Route>
            <Route path="/products" element={<ProductsTable />}></Route>
            <Route path="/orders" element={<OrdersTable />}></Route>
            <Route path="/customers" element={<CustomersTable />}></Route>
          </Routes>
        </div>
      </div>
    </div>
  );
};

export default Admin;
