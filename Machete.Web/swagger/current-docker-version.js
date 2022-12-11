#!/usr/bin/node
import fs from "fs";
import https from "https";
import path from "path";
import { fileURLToPath } from "url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const onSuccess = (jsondata) => {
  const { results } = JSON.parse(jsondata);

  const [, currentTag] = results;

  const latestVersion = currentTag.name;
  console.log(__dirname);
  fs.writeFile(
    `${__dirname}/curent-docker-version.txt`,
    latestVersion,
    (err) => {
      if (err) {
        console.error(err);
      }
    }
  );
};

const main = () => {
  // See: https://docs.docker.com/docker-hub/api/latest/#tag/repositories/paths/~1v2~1namespaces~1%7Bnamespace%7D~1repositories~1%7Brepository%7D~1tags/get
  https
    .get(
      "https://hub.docker.com/v2/namespaces/ndlonmachete/repositories/debian/tags",
      (res) => {
        res.on("data", onSuccess);
      }
    )
    .on("error", function (e) {
      console.error(e);
    });
};

main();
