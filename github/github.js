const Octokit = require('@octokit/rest');
const github = new Octokit();

// the operation to perform
const operation = process.argv[2];
const appveyorBuildNumber = process.env.APPVEYOR_BUILD_VERSION;
// TODO process.env.MACHETE_REPO_OWNER etc.
const owner = 'chaim1221';
const commitish = 'master'; // default

// GITHUB
var createRelease = function (repo, tag) {
  return new Promise(function (resolve, reject) {
    github.repos.createRelease({
      owner: owner,
      repo: repo,
      target_commitish: commitish,
      tag_name: appveyorBuildNumber,
      name: appveyorBuildNumber,
      body: 'The latest Machete release. Created by AppVeyor.',
      draft: false,
      prerelease: false
    }, (error, response) => {
      if (error) return reject({ code: error.code, status: error.status || error.code == 401 ? error : "Already Exists", repo, ref: tag });
      resolve(response.headers.status + " " + tag + " in " + repo);
    });
  });
}

// should not be called for non-https errors; TODO: make sure that is happening
var handleHttpsError = function (error) {
  return "Error: " + error.code + " " + error.status + " ("  + error.repo + ": " + error.ref + ")";
}
// SERVICE
var createMacheteRelease = function (tag) {
  return createRelease("Machete", tag).catch(e => handleHttpsError(e));
}

// MAIN
function main() {
  let gitToken = process.env.GIT_TOKEN;
  github.authenticate({
    type: 'token',
    token: gitToken
  });
  switch (operation) {
    case "create-machete-release":
      createMacheteRelease(appveyorBuildNumber).then(result => console.log(result));
      break;
    default:
      throw "Operation \"" + operation + "\" is not implemented."
  }
}

main();
