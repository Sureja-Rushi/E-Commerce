import { Button, Grid, TextField } from "@mui/material";
import React, { useEffect } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router";
import { login } from "../../State/Auth/Action";

const LoginForm = () => {

  const navigate = useNavigate();
  const dispatch = useDispatch();

  const handleSubmit = (e) => {
    e.preventDefault();
    const data = new FormData(e.currentTarget);
    const userData = {
      email: data.get("email"),
      password: data.get("password"),
    };

    dispatch(login(userData));
    console.log(userData);
  };

  // useEffect(() => {
  //   if(jwt){
  //     dispatch(getUser())
  //   }
  // },[jwt, auth.jwt])

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <TextField
              required
              id="email"
              name="email"
              label="Email"
              fullWidth
              autoComplete="email"
            />
          </Grid>

          <Grid item xs={12}>
            <TextField
              required
              id="password"
              name="password"
              label="Password"
              fullWidth
              autoComplete="password"
            />
          </Grid>

          <Grid item xs={12}>
            <Button
              className="bg-[#9155fd] w-full"
              type="submit"
              variant="contained"
              size="large"
              sx={{ padding: ".8rem 0", bgcolor: "#9155fd" }}
            >
              Login
            </Button>
          </Grid>
        </Grid>
      </form>

      <div className="flex flex-col justify-center items-center">
        <div className="py-3 flex items-center">
          <p>Don't have an account ?</p>
          <Button
            onClick={() => navigate("/register")}
            className="ml-5"
            size="small"
          >
            Register
          </Button>
        </div>
      </div>
    </div>
  );
};

export default LoginForm;
